//------------------------------------------------------------------------------
// <copyright file="GestureDetector.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.DiscreteGestureBasics
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Kinect;
    using Microsoft.Kinect.VisualGestureBuilder;
    using mmisharp;

    /// <summary>
    /// Gesture Detector class which listens for VisualGestureBuilderFrame events from the service
    /// and updates the associated GestureResultView object with the latest results for the 'Seated' gesture
    /// </summary>
    public class GestureDetector : IDisposable
    {
        /// <summary> Path to the gesture database that was trained with VGB </summary>
        //private readonly string gestureDatabase = @"Database\Seated.gbd";
        private readonly string gestureDatabase = @"Database\Gestures.gbd";

        /// <summary> Name of the discrete gesture in the database that we want to track </summary>
        private readonly string hotelGestureName = "Hotel";
        private readonly string flightGestureName = "flight";
        private readonly string selectGestureName = "Select";
        private readonly string upGestureName = "Up_Right";
        private readonly string DownGestureName = "Down_Left";

        /// <summary> Gesture frame source which should be tied to a body tracking ID </summary>
        private VisualGestureBuilderFrameSource vgbFrameSource = null;

        /// <summary> Gesture frame reader which will handle gesture events coming from the sensor </summary>
        private VisualGestureBuilderFrameReader vgbFrameReader = null;

        private LifeCycleEvents lce;
        private MmiCommunication mmic;

        private MainWindow main;

        /// <summary>
        /// Initializes a new instance of the GestureDetector class along with the gesture frame source and reader
        /// </summary>
        /// <param name="kinectSensor">Active sensor to initialize the VisualGestureBuilderFrameSource object with</param>
        /// <param name="gestureResultView">GestureResultView object to store gesture results of a single body to</param>
        public GestureDetector(KinectSensor kinectSensor, GestureResultView gestureResultView, MainWindow main)
        {
            if (kinectSensor == null)
            {
                throw new ArgumentNullException("kinectSensor");
            }

            if (gestureResultView == null)
            {
                throw new ArgumentNullException("gestureResultView");
            }

            this.main = main;
            this.GestureResultView = gestureResultView;
            
            // create the vgb source. The associated body tracking ID will be set when a valid body frame arrives from the sensor.
            this.vgbFrameSource = new VisualGestureBuilderFrameSource(kinectSensor, 0);
            this.vgbFrameSource.TrackingIdLost += this.Source_TrackingIdLost;

            // open the reader for the vgb frames
            this.vgbFrameReader = this.vgbFrameSource.OpenReader();
            if (this.vgbFrameReader != null)
            {
                this.vgbFrameReader.IsPaused = true;
                this.vgbFrameReader.FrameArrived += this.Reader_GestureFrameArrived;
            }

            // load the 'Seated' gesture from the gesture database
            using (VisualGestureBuilderDatabase database = new VisualGestureBuilderDatabase(this.gestureDatabase))
            {
                //this.vgbFrameSource.AddGesture(database.AvailableGestures);

                // we could load all available gestures in the database with a call to vgbFrameSource.AddGestures(database.AvailableGestures), 
                // but for this program, we only want to track one discrete gesture from the database, so we'll load it by name
                
                foreach (Gesture gesture in database.AvailableGestures)
                {
                    this.vgbFrameSource.AddGesture(gesture);
                }
            }

            //init LifeCycleEvents..
            lce = new LifeCycleEvents("ASR", "FUSION", "speech-1", "acoustic", "command"); // LifeCycleEvents(string source, string target, string id, string medium, string mode)
            //mmic = new MmiCommunication("localhost",9876,"User1", "ASR");  //PORT TO FUSION - uncomment this line to work with fusion later
            mmic = new MmiCommunication("localhost", 8000, "User1", "ASR"); // MmiCommunication(string IMhost, int portIM, string UserOD, string thisModalityName)

            mmic.Send(lce.NewContextRequest());
        }

        /// <summary> Gets the GestureResultView object which stores the detector results for display in the UI </summary>
        public GestureResultView GestureResultView { get; private set; }

        /// <summary>
        /// Gets or sets the body tracking ID associated with the current detector
        /// The tracking ID can change whenever a body comes in/out of scope
        /// </summary>
        public ulong TrackingId
        {
            get
            {
                return this.vgbFrameSource.TrackingId;
            }

            set
            {
                if (this.vgbFrameSource.TrackingId != value)
                {
                    this.vgbFrameSource.TrackingId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the detector is currently paused
        /// If the body tracking ID associated with the detector is not valid, then the detector should be paused
        /// </summary>
        public bool IsPaused
        {
            get
            {
                return this.vgbFrameReader.IsPaused;
            }

            set
            {
                if (this.vgbFrameReader.IsPaused != value)
                {
                    this.vgbFrameReader.IsPaused = value;
                }
            }
        }

        /// <summary>
        /// Disposes all unmanaged resources for the class
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader objects
        /// </summary>
        /// <param name="disposing">True if Dispose was called directly, false if the GC handles the disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.vgbFrameReader != null)
                {
                    this.vgbFrameReader.FrameArrived -= this.Reader_GestureFrameArrived;
                    this.vgbFrameReader.Dispose();
                    this.vgbFrameReader = null;
                }

                if (this.vgbFrameSource != null)
                {
                    this.vgbFrameSource.TrackingIdLost -= this.Source_TrackingIdLost;
                    this.vgbFrameSource.Dispose();
                    this.vgbFrameSource = null;
                }
            }
        }

        /// <summary>
        /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
        {
            VisualGestureBuilderFrameReference frameReference = e.FrameReference;
            using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    // get the discrete gesture results which arrived with the latest frame
                    IReadOnlyDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;

                    if (discreteResults != null)
                    {
                        // we only have one gesture in this source object, but you can get multiple gestures
                        foreach (Gesture gesture in this.vgbFrameSource.Gestures)
                        {
                            if (gesture.Name.Equals(this.hotelGestureName) && gesture.GestureType == GestureType.Discrete)
                            {
                                DiscreteGestureResult result = null;
                                discreteResults.TryGetValue(gesture, out result);

                                if (result != null)
                                {
                                    Console.WriteLine(result.Confidence);
                                    if (result.Confidence > 0.6)
                                    {
                                        Console.WriteLine(" HOTEL GESTURE DETECTED ");
                                        //main.updateList(1);
                                        SendCommand("HOTEL");
                                    }
                                    // update the GestureResultView object with new gesture result values
                                    this.GestureResultView.UpdateGestureResult(true, result.Detected, result.Confidence , this.hotelGestureName);
                                }
                            }

                            if (gesture.Name.Equals(this.flightGestureName) && gesture.GestureType == GestureType.Discrete)
                            {
                                DiscreteGestureResult result = null;
                                discreteResults.TryGetValue(gesture, out result);

                                if (result != null)
                                {
                                    Console.WriteLine(result.Confidence);
                                    if (result.Confidence > 0.6)
                                    {
                                        Console.WriteLine(" FLIGHT GESTURE DETECTED ");
                                        //main.updateList(2);
                                        SendCommand("FLIGHT");
                                    }
                                    // update the GestureResultView object with new gesture result values
                                    this.GestureResultView.UpdateGestureResult(true, result.Detected, result.Confidence, this.flightGestureName);
                                }
                            }

                            if (gesture.Name.Equals(this.selectGestureName) && gesture.GestureType == GestureType.Discrete)
                            {
                                DiscreteGestureResult result = null;
                                discreteResults.TryGetValue(gesture, out result);

                                if (result != null)
                                {
                                    Console.WriteLine(result.Confidence);
                                    if (result.Confidence > 0.4)
                                    {
                                        Console.WriteLine(" SELECT GESTURE DETECTED ");
                                        main.updateList(3);
                                        //MouseHook.SendClick();
                                    }
                                    // update the GestureResultView object with new gesture result values
                                    this.GestureResultView.UpdateGestureResult(true, result.Detected, result.Confidence, this.selectGestureName);
                                }
                            }

                            if (gesture.Name.Equals(this.upGestureName) && gesture.GestureType == GestureType.Discrete)
                            {
                                DiscreteGestureResult result = null;
                                discreteResults.TryGetValue(gesture, out result);

                                if (result != null)
                                {
                                    Console.WriteLine(result.Confidence);
                                    if (result.Confidence > 0.4)
                                    {
                                        Console.WriteLine(" UP GESTURE DETECTED ");
                                        main.updateList(2);
                                    }
                                    // update the GestureResultView object with new gesture result values
                                    this.GestureResultView.UpdateGestureResult(true, result.Detected, result.Confidence, this.upGestureName);
                                }
                            }

                            if (gesture.Name.Equals(this.DownGestureName) && gesture.GestureType == GestureType.Discrete)
                            {
                                DiscreteGestureResult result = null;
                                discreteResults.TryGetValue(gesture, out result);

                                if (result != null)
                                {
                                    Console.WriteLine(result.Confidence);
                                    if (result.Confidence > 0.4)
                                    {
                                        Console.WriteLine(" DOWN GESTURE DETECTED ");
                                        main.updateList(1);
                                    }
                                    // update the GestureResultView object with new gesture result values
                                    this.GestureResultView.UpdateGestureResult(true, result.Detected, result.Confidence, this.DownGestureName);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SendCommand(string command)
        {
            //SEND
            // IMPORTANT TO KEEP THE FORMAT {"recognized":["SEARCH","FLIGHT"]}
            string json = "{ \"recognized\":[\"SEARCH\",\"" + command + "\"] }";

            var exNot = lce.ExtensionNotification("", "", 100, json);
            mmic.Send(exNot);
        }

        /// <summary>
        /// Handles the TrackingIdLost event for the VisualGestureBuilderSource object
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Source_TrackingIdLost(object sender, TrackingIdLostEventArgs e)
        {
            // update the GestureResultView object to show the 'Not Tracked' image in the UI
            this.GestureResultView.UpdateGestureResult(false, false, 0.0f, null);
        }
    }
}
