//------------------------------------------------------------------------------
//The MIT License (MIT)
//Copyright (c) 2016 David Jardim
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//-----------------------------------------------------------------------------
namespace SkeletonViewer
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Microsoft.Kinect;    
    using System.Collections.Generic;
    using System;


    /// <summary>
    /// Class responsible for drawing the skeleton (based on the SkeletonBasics sample from https://www.microsoft.com/en-us/download/details.aspx?id=40278)
    /// </summary>
    public class DisplayManager
    {
        /// <summary>
        /// Width of output drawing
        /// </summary>
        private const float RenderWidth = 640.0f;

        /// <summary>
        /// Height of our output drawing
        /// </summary>
        private const float RenderHeight = 480.0f;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 3;            
       

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));       
               
        private readonly Pen pen = new Pen(Brushes.Yellow, 1);

        /// <summary>
        /// Drawing group for skeleton rendering output
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage imageSource;

        public DisplayManager(Image image)
        {
            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            // Display the drawing using our image control
            image.Source = this.imageSource;
        }

        /// <summary>
        /// Maps a SkeletonPoint to lie within our render space and converts to Point
        /// </summary>
        /// <param name="skelpoint">point to map</param>
        /// <returns>mapped point</returns>
        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            return new Point((skelpoint.X * 320) + 320, (skelpoint.Y * 240) + 240);
        }

        /// <summary>
        /// Draws the skeleton on the canvas
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="actionId"></param>
        /// <param name="skeletonDataFrame"></param>
        internal void DrawSkeleton(int frame, int actionId, Dictionary<JointType, Vector3> skeletonDataFrame)
        {
            using (DrawingContext dc = this.drawingGroup.Open())
            {
                // Draw a transparent background to set the render size
                dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                Utils.FlipSkeleton(skeletonDataFrame);
                this.DrawBonesAndJoints(skeletonDataFrame, dc);
                this.DrawHud(dc, frame, actionId);
            }
            // prevent drawing outside of our render area
            this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));

        }

        /// <summary>
        /// Draws the HUD information on the canvas
        /// </summary>
        /// <param name="drawingContext"></param>
        /// <param name="frame"></param>
        /// <param name="actionId"></param>
        private void DrawHud(DrawingContext drawingContext, int frame, int actionId)
        {
            drawingContext.DrawText(Utils.CreateText(String.Format("Frame: {0}", frame)), new Point());
            drawingContext.DrawText(Utils.CreateText(String.Format("Action: {0}", Utils.GetActionNameFromId(actionId))), new Point(0, 24));
        }

        /// <summary>
        /// Draws a skeleton's bones and joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawBonesAndJoints(Dictionary<JointType, Vector3> skeleton, DrawingContext drawingContext)
        {
            // Render Torso
            this.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
            this.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

            // Left Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

            // Right Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

            // Left Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);

            // Render Joints
            foreach (Vector3 joint in skeleton.Values)
            {
                Brush drawBrush = null;
                drawBrush = this.trackedJointBrush;
                if (drawBrush != null)
                {
                    var skeletonPoint = new SkeletonPoint();
                    skeletonPoint.X = joint.X;
                    skeletonPoint.Y = joint.Y;
                    skeletonPoint.Z = joint.Z;
                    drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(skeletonPoint), JointThickness, JointThickness);
                }
            }
        }

        /// <summary>
        /// Draws a bone line between two joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw bones from</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="jointType0">joint to start drawing from</param>
        /// <param name="jointType1">joint to end drawing at</param>
        private void DrawBone(Dictionary<JointType, Vector3> skeleton, DrawingContext drawingContext, JointType jointType1, JointType jointType2)
        {
            Vector3 joint0 = skeleton[jointType1];
            Vector3 joint1 = skeleton[jointType2];

            var p0 = new Point((joint0.X * 320) + 320, (joint0.Y * 240) + 240);
            var p1 = new Point((joint1.X * 320) + 320, (joint1.Y * 240) + 240);            
            
            drawingContext.DrawLine(pen, p0, p1);
        }
    }
}