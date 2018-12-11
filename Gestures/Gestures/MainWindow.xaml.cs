//---------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <Description>
// This program tracks up to 6 people simultaneously.
// If a person is tracked, the associated gesture detector will determine if that person is seated or not.
// If any of the 6 positions are not in use, the corresponding gesture detector(s) will be paused
// and the 'Not Tracked' image will be displayed in the UI.
// </Description>
//----------------------------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.DiscreteGestureBasics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Microsoft.Kinect;
    using Microsoft.Kinect.VisualGestureBuilder;
    using System.Drawing;
    using mmisharp;

    /// <summary>
    /// Interaction logic for the MainWindow
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary> Active Kinect sensor </summary>
        private KinectSensor kinectSensor = null;
        
        /// <summary> Array for the bodies (Kinect will track up to 6 people simultaneously) </summary>
        private Body[] bodies = null;

        /// <summary> Reader for body frames </summary>
        private BodyFrameReader bodyFrameReader = null;

        /// <summary> Current status text to display </summary>
        private string statusText = null;

        /// <summary> KinectBodyView object which handles drawing the Kinect bodies to a View box in the UI </summary>
        private KinectBodyView kinectBodyView = null;
        
        /// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
        private List<GestureDetector> gestureDetectorList = null;

        private List<TodoItem> types = new List<TodoItem>();
        private List<TodoItem> destinations = new List<TodoItem>();
        private MainWindow main;
        //private int clickCounter; // Not used
        private string typeSelected;
        private string destinationSelected;

        private LifeCycleEvents lce;
        private MmiCommunication mmic;

        
        public void updateList(int list)
        {
            Console.WriteLine("CHEGOU AQUIIIII");
            if (lbTodoList.ItemsSource == types)
            {
                Console.WriteLine("TYPEPPEPEPEPEPEPEPEPEPPEPPAKLFNAKLSJNKJLASBKJ");
            }

            if (list == 1)  //DOWN
            {
                types[0].Color = "";
                types[1].Color = "#ff00BCF2";
            }
            if (list == 2)  //UP
            {
                lbTodoList.ItemsSource = types;
                types[0].Color = "#ff00BCF2";
                types[1].Color = "";
            }
            if (list == 3)  //SELECT
            {
                Console.WriteLine("SELECTTTT");
                lbTodoList.ItemsSource = destinations;
            }
        }
        

        private void ListView1_Click(object sender, EventArgs e)
        {
            /*clickCounter++;

            if ( clickCounter == 2)
            {
                lbTodoList.SelectedItems.Clear();
                lbTodoList.ItemsSource = types;
            }

            if (clickCounter == 1)
            {
                lbTodoList.SelectedItems.Clear();
                lbTodoList.ItemsSource = destinations;
            }
            */


            //var firstSelectedItem = lbTodoList.SelectedItems[0];
            //Console.WriteLine(firstSelectedItem);

            foreach (TodoItem selected in lbTodoList.SelectedItems)
            {
                foreach (TodoItem item in types)
                {
                    if (selected.Equals(item))
                    {
                        Console.WriteLine(item.Title);
                        if (item.Title.Contains("Voo"))
                        {
                            typeSelected = "FLIGHT";
                            types[0].Color = "#ff52318f";
                        }
                        else
                        {
                            typeSelected = "HOTEL";
                        }
                    }
                }
                /*
                foreach (TodoItem destination in destinations)
                {
                    if (selected.Equals(destination))
                    {
                        Console.WriteLine(destination.Title);
                        if (destination.Title.Contains("Paris"))
                        {
                            destinationSelected = "PARIS";
                        }
                        else if(destination.Title.Contains("Londres"))
                        {
                            destinationSelected = "LONDON";
                        }
                        else if (destination.Title.Contains("Roma"))
                        {
                            destinationSelected = "ROME";
                        }
                        SendCommand(typeSelected, destinationSelected);
                    }
                }
            }

            lbTodoList.SelectedItems.Clear();

            lbTodoList.ItemsSource = destinations;*/
            }
        }

        private void SendCommand(string type, string destination)
        {
            //SEND
            // IMPORTANT TO KEEP THE FORMAT {"recognized":["FLIGHT","PARIS"]}
            string json = "{ \"recognized\":[\"" + type + "\",\"" + destination + "\"] }";

            var exNot = lce.ExtensionNotification("", "", 100, json);
            mmic.Send(exNot);
        }

        public MainWindow()
        {
            main = this;

            InitializeComponent();

            types.Add(new TodoItem() { Title = "Pesquisar Voo" , Color = "#ff00BCF2" });
            types.Add(new TodoItem() { Title = "Pesquisar Hotel" });

            destinations.Add(new TodoItem() { Title = "Paris", Color = "#ff00BCF2" });
            destinations.Add(new TodoItem() { Title = "Londres" });
            destinations.Add(new TodoItem() { Title = "Roma" });

            lbTodoList.ItemsSource = types;

            // only one sensor is currently supported
            this.kinectSensor = KinectSensor.GetDefault();
            
            // set IsAvailableChanged event notifier
            this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            // open the sensor
            this.kinectSensor.Open();

            // set the status text
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.NoSensorStatusText;

            // open the reader for the body frames
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // set the BodyFramedArrived event notifier
            this.bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;

            // initialize the BodyViewer object for displaying tracked bodies in the UI
            this.kinectBodyView = new KinectBodyView(this.kinectSensor);

            // initialize the gesture detection objects for our gestures
            this.gestureDetectorList = new List<GestureDetector>();

            // initialize the MainWindow
            this.InitializeComponent();

            // set our data context objects for display in UI
            this.DataContext = this;
            this.kinectBodyViewbox.DataContext = this.kinectBodyView;

            // create a gesture detector for each body (6 bodies => 6 detectors) and create content controls to display results in the UI
            //int col0Row = 0;
            //int col1Row = 0;
            //int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
            //for (int i = 0; i < maxBodies; ++i)
            //{
                GestureResultView result = new GestureResultView(0, false, false, 0.0f);
                GestureDetector detector = new GestureDetector(this.kinectSensor, result, this.main);
                this.gestureDetectorList.Add(detector);                
                
                // split gesture results across the first two columns of the content grid
                ContentControl contentControl = new ContentControl();
                contentControl.Content = this.gestureDetectorList[0].GestureResultView;
                
                //if (i % 2 == 0)
                //{
                    // Gesture results for bodies: 0, 2, 4
                    Grid.SetColumn(contentControl, 0);
                    Grid.SetRow(contentControl, 2);
                    //++col0Row;
                //}
                //else
                //{
                   // Gesture results for bodies: 1, 3, 5
                   // Grid.SetColumn(contentControl, 1);
                   //Grid.SetRow(contentControl, col1Row);
                   // ++col1Row;
                //}

                this.contentGrid.Children.Add(contentControl);
            //}

            //init LifeCycleEvents..
            lce = new LifeCycleEvents("ASR", "FUSION", "speech-1", "acoustic", "command"); // LifeCycleEvents(string source, string target, string id, string medium, string mode)
            //mmic = new MmiCommunication("localhost",9876,"User1", "ASR");  //PORT TO FUSION - uncomment this line to work with fusion later
            mmic = new MmiCommunication("localhost", 8000, "User1", "ASR"); // MmiCommunication(string IMhost, int portIM, string UserOD, string thisModalityName)

            mmic.Send(lce.NewContextRequest());
        }

        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the current status text to display
        /// </summary>
        public string StatusText
        {
            get
            {
                return this.statusText;
            }

            set
            {
                if (this.statusText != value)
                {
                    this.statusText = value;

                    // notify any bound elements that the text has changed
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                    }
                }
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.bodyFrameReader != null)
            {
                // BodyFrameReader is IDisposable
                this.bodyFrameReader.FrameArrived -= this.Reader_BodyFrameArrived;
                this.bodyFrameReader.Dispose();
                this.bodyFrameReader = null;
            }

            if (this.gestureDetectorList != null)
            {
                // The GestureDetector contains disposable members (VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader)
                foreach (GestureDetector detector in this.gestureDetectorList)
                {
                    detector.Dispose();
                }

                this.gestureDetectorList.Clear();
                this.gestureDetectorList = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.IsAvailableChanged -= this.Sensor_IsAvailableChanged;
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }

        /// <summary>
        /// Handles the event when the sensor becomes unavailable (e.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            // on failure, set the status text
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.SensorNotAvailableStatusText;
        }

        /// <summary>
        /// Handles the body frame data arriving from the sensor and updates the associated gesture detector object for each body
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                    {
                        // creates an array of 6 bodies, which is the max number of bodies that Kinect can track simultaneously
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);

                    foreach(var body in bodies)
                    {
                        if(body != null)
                        {
                            if (body.IsTracked)
                            {
                                Joint handRight = body.Joints[JointType.HandRight];

                                CameraSpacePoint skeletonPoint = handRight.Position;

                                DepthSpacePoint depthPoint = this.kinectSensor.CoordinateMapper.MapCameraPointToDepthSpace(skeletonPoint);
                                /*
                                this.Dispatcher.Invoke(() =>
                                {
                                    cursor.Flip(handRight);
                                    cursor.Update(depthPoint);
                                });*/
                            }
                        }
                    }

                    dataReceived = true;
                }
                else
                {
                    Console.WriteLine("bodyFrame is null");
                }
            }

            if (dataReceived)
            {
                // visualize the new body data
                this.kinectBodyView.UpdateBodyFrame(this.bodies);

                // we may have lost/acquired bodies, so update the corresponding gesture detectors
                if (this.bodies != null)
                {
                    // loop through all bodies to see if any of the gesture detectors need to be updated
                    int maxBodies = 1; // this.kinectSensor.BodyFrameSource.BodyCount;
                    for (int i = 0; i < maxBodies; ++i)
                    {
                        Body body = this.bodies[i];
                        ulong trackingId = body.TrackingId;

                        // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                        if (trackingId != this.gestureDetectorList[i].TrackingId)
                        {
                            this.gestureDetectorList[i].TrackingId = trackingId;

                            // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                            // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                            this.gestureDetectorList[i].IsPaused = trackingId == 0;
                        }
                    }
                }
            }
        }
    }
}
