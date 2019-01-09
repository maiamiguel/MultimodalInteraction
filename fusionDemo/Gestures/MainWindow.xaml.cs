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

        private List<TodoItem> options = new List<TodoItem>();
        private MainWindow main;
        private string typeSelected;
        private string destinationSelected;

        private LifeCycleEvents lce;
        private MmiCommunication mmic;
        private int position = 0;

        public void updateList(int list){
            Console.WriteLine("--------------------------------------------------------------------------\n");
            Console.WriteLine("OPTIONS ARRAY BEING DISPLAYED!!");
            Console.WriteLine("--------------------------------------------------------------------------\n");

            Console.WriteLine("POSITION -> : ");
            Console.WriteLine(position);

            if (position == 0)
            {
                position = 0;
            }

            if (position == 5)
            {
                position = 5;
            }

            if (list == 1 && position < 5)  //DOWN
            {
                foreach(TodoItem i in options)
                {
                    i.Color = "";
                }
                position++;
                options[position].Color = "#ff00BCF2";
            }

            if (list == 2 && position > 0)  //UP
            {
                foreach (TodoItem i in options)
                {
                    i.Color = "";
                }
                position--;
                options[position].Color = "#ff00BCF2";
            }
            if (list == 3)  //SELECT
            {
                Console.WriteLine("SELECT !");
                SendCommand(position);
            }
        }

        public MainWindow(){
            main = this;

            InitializeComponent();

            options.Add(new TodoItem() { Title = "Pesquisar Voo para Paris" , Color = "#ff00BCF2" });
            options.Add(new TodoItem() { Title = "Pesquisar Voo para Roma" });
            options.Add(new TodoItem() { Title = "Pesquisar Voo para Londres" });
            options.Add(new TodoItem() { Title = "Pesquisar Hotel para Paris" });
            options.Add(new TodoItem() { Title = "Pesquisar Hotel para Roma" });
            options.Add(new TodoItem() { Title = "Pesquisar Hotel para Londres" });

            lbTodoList.ItemsSource = options;

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
                GestureDetector detector = new GestureDetector(this.kinectSensor, result, this.main, circle, this.Dispatcher);
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
            lce = new LifeCycleEvents("TOUCH", "FUSION", "touch-1", "touch", "command"); // LifeCycleEvents(string source, string target, string id, string medium, string mode)
            mmic = new MmiCommunication("localhost",9876,"User1", "TOUCH");  //PORT TO FUSION - uncomment this line to work with fusion later
            //mmic = new MmiCommunication("localhost", 8000, "User1", "ASR"); // MmiCommunication(string IMhost, int portIM, string UserOD, string thisModalityName)

            mmic.Send(lce.NewContextRequest());
        }

        private void SendCommand(int position)
        {
            if (position == 0)
            {
                typeSelected = "FLIGHT";
                destinationSelected = "PARIS";
            }
            if (position == 1)
            {
                typeSelected = "FLIGHT";
                destinationSelected = "ROME";
            }
            if (position == 2)
            {
                typeSelected = "FLIGHT";
                destinationSelected = "LONDON";
            }
            if (position == 3)
            {
                typeSelected = "HOTEL";
                destinationSelected = "PARIS";
            }
            if (position == 4)
            {
                typeSelected = "HOTEL";
                destinationSelected = "ROME";
            }
            if (position == 5)
            {
                typeSelected = "HOTEL";
                destinationSelected = "LONDON";
            }

            //SEND
            // IMPORTANT TO KEEP THE FORMAT {"recognized":["FLIGHT","PARIS"]}
            string json = "{ \"recognized\":[\"" + typeSelected + "\",\"" + destinationSelected + "\"] }";

            var exNot = lce.ExtensionNotification("", "", 100, json);
            mmic.Send(exNot);
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
