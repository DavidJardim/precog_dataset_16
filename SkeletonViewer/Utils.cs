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
//------------------------------------------------------------------------------
namespace SkeletonViewer
{
    using Microsoft.Kinect;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Class that holds utilitary methods
    /// </summary>
    public class Utils
    {

        /// <summary>
        /// Enumerate of actions contained in the dataset
        /// </summary>
        public enum Actions
        {
            None,
            RightPunch,
            LeftPunch,
            FrontRightKick,
            FrontLeftKick,
            SideRightKick,
            SideLeftKick,
            BackFist,
            ElbowStrike
        }

        /// <summary>
        /// Returns the action name given the action id
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public static string GetActionNameFromId(int actionId)
        {
            switch (actionId)
            {
                case 1:
                    return Actions.RightPunch.ToString(); //1
                case 2:
                    return Actions.LeftPunch.ToString(); //2
                case 3:
                    return Actions.FrontRightKick.ToString(); //3
                case 4:
                    return Actions.FrontLeftKick.ToString(); //4
                case 5:
                    return Actions.SideRightKick.ToString(); //5
                case 6:
                    return Actions.SideLeftKick.ToString(); //6
                case 7:
                    return Actions.BackFist.ToString(); //7
                case 8:
                    return Actions.ElbowStrike.ToString(); //8
                default:
                    return Actions.None.ToString();
            }
        }

        /// <summary>
        /// Creates a formatted text to be rendered in the canvas
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FormattedText CreateText(string value)
        {
            FormattedText formattedText = new FormattedText(
                      value,
                      CultureInfo.GetCultureInfo("en-us"),
                      FlowDirection.LeftToRight,
                      new Typeface("Verdana"),
                      24,
                      Brushes.White);

            return formattedText;
        }
        /// <summary>
        /// Flips the skeleton's to match the view of the user
        /// </summary>
        /// <param name="skeleton"></param>
        public static void FlipSkeleton(Dictionary<JointType, Vector3> skeleton)
        {
            if (null == skeleton)
            {
                return;
            }

            Array jointTypeValues = Enum.GetValues(typeof(JointType));

            foreach (JointType j in jointTypeValues)
            {
                Vector3 mirroredjointPosition = skeleton[j];

                // Here we negate the Z or X axis to change the skeleton to mirror the user's movements.
                // Note that this potentially requires us to re-position our camera
                mirroredjointPosition.X = -mirroredjointPosition.X;
                mirroredjointPosition.Y = -mirroredjointPosition.Y;
                skeleton[j] = mirroredjointPosition;
            }
        }
    }
}