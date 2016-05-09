namespace SkeletonViewer
{
    using Microsoft.Win32;
    using System;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DisplayManager displayManager;
        private DispatcherTimer timer;
        private Sequence sequence;
        private int frameCounter;
        private string selectedSequenceFile;

        /// <summary>
        /// Initialization of the application
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            frameCounter = 0;
            // 25 fps -> 1000ms / 25 = 40ms
            timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 40) };
            timer.Tick += timer_Tick;
            displayManager = new DisplayManager(this.KinectCanvas);
        }

        /// <summary>
        /// Event triggered on each tick of the timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            if (frameCounter < sequence.SkeletonDataFrames.Count)
            {
                int actionId = sequence.SkeletonDataFrames[frameCounter].ActionId;
                displayManager.DrawSkeleton(frameCounter, actionId, sequence.SkeletonDataFrames[frameCounter].Joints);                
                frameCounter++;
            }
            else
            {
                this.Stop();
            }
        }
        
        /// <summary>
        /// Method to stop the playing of the sequence
        /// </summary>
        private void Stop()
        {                                                
            timer.Stop();
            frameCounter = 0;
            selectedSequenceFile = null;            
            labelSequenceName.Content = "None";
            ButtonPlay.IsEnabled = true;
        }

        /// <summary>
        /// Event to open the file dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSelectFile_Click(object sender, RoutedEventArgs e)
        {
            this.Stop();
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select filename",
                Filter = "CSV files|*.csv"
            };
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() != true) return;
            selectedSequenceFile = openFileDialog.FileName;
            labelSequenceName.Content = selectedSequenceFile;            
        }

        /// <summary>
        /// Event triggered when the play button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sequence = new Sequence(selectedSequenceFile);
                ButtonPlay.IsEnabled = false;
                timer.Start();
            }
            catch (Exception ex)
            {
                labelSequenceName.Content = ex.Message;
            }
        }

        /// <summary>
        /// /// Event triggered when the stop button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            this.Stop();
        }
    }
}