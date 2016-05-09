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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;    
    using System.Globalization;
    using Microsoft.Kinect;

    /// <summary>
    /// This class represents a sequence of action
    /// </summary>
    class Sequence
    {
        /// <summary>
        /// The frames of the sequence
        /// </summary>
        public List<SkeletonDataFrame> SkeletonDataFrames { get; set; }
        /// <summary>
        /// This method will parse a CSV file into a sequence
        /// </summary>
        /// <param name="sequenceFile">The CSV file to read</param>
        public Sequence(string sequenceFile)
        {
            SkeletonDataFrames = new List<SkeletonDataFrame>();
            var AnnotatedFrames = new List<int>();
            List<string> headers = new List<string>();

            var sequenceContent = File.ReadAllText(sequenceFile);
            var lines = sequenceContent.Split('\n');
            headers.AddRange(lines[0].Split(';'));
            for (int i = 1; i < lines.Length; i++)
            {
                var skeletonData = new SkeletonDataFrame();
                var line = lines[i];
                string[] columns = line.Split(';');
                AnnotatedFrames.Add(Int32.Parse(columns.Last()));
                for (int j = 0; j < columns.Length - 1; j++)
                {
                    JointType joint = (JointType)Enum.Parse(typeof(JointType), headers[j], true);

                    if (!skeletonData.Joints.ContainsKey(joint))
                    {
                        skeletonData.Joints.Add(joint, new Vector3());
                    }
                    var pos_str = columns[j].Split(' ');

                    float x = float.Parse(pos_str[0], CultureInfo.InvariantCulture);
                    float y = float.Parse(pos_str[1], CultureInfo.InvariantCulture);
                    float z = float.Parse(pos_str[2], CultureInfo.InvariantCulture);
                    Vector3 position = new Vector3((float)x, y, z);

                    skeletonData.Joints[joint] = position;
                }
                SkeletonDataFrames.Add(skeletonData);
            }
            for (int k = 0; k < AnnotatedFrames.Count; k++)
            {
                int actionId = AnnotatedFrames[k];
                SkeletonDataFrames[k].ActionId = actionId;
            }
        }
    }
}