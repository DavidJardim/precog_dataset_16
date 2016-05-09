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
    using System.Collections.Generic;

    /// <summary>
    /// This class represents a frame of the sequence
    /// </summary>
    class SkeletonDataFrame
    {
        /// <summary>
        /// This dictionary represents all the joints contained in the skeleton and it's 3D position
        /// </summary>
        public Dictionary<JointType, Vector3> Joints { get; set; }
        /// <summary>
        /// The ID of the action that was manually tagged
        /// </summary>
        public int ActionId { get; set; }

        public SkeletonDataFrame()
        {
            ActionId = 0;
            Joints = new Dictionary<JointType, Vector3>();
        }
    }
}
