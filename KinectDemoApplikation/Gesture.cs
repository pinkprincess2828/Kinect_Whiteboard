﻿using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectDemoApplikation
{
    public class Gesture
    {
        //can be used for creating gesture pattern enums in controller
        public static GesturePart open; 
        public static GesturePart closed;

        //containing the gesture parts
        private IGesturePart[] parts;
        //just for debugging
        public String name;

        //points on the currently analyzed part
        private int partCounter = 0;

        //event handler, firing the event in case of a matched gesture
        public event EventHandler GestureRecognized;

        /// <summary>
        /// Default constructor 
        /// </summary>
        /// 
        public Gesture(IGesturePart[] pattern) {
            open = new GesturePart(Microsoft.Kinect.HandState.Open);
            closed = new GesturePart(Microsoft.Kinect.HandState.Closed);
            parts = pattern;
        }


        public void Update(Body body)
        {

            //Debug.WriteLine(this.name + ":" + partCounter);
            GesturePartResult result = parts[this.partCounter].CheckGesturePart(body);


            //Console.WriteLine(result);

            //if result has been positive
            if (result == GesturePartResult.Succeded)
            {

                //increase counter and check whether gesture is complete
                if (++partCounter == parts.Length)
                {
                    partCounter = 0;
                    //fire event
                    if (GestureRecognized != null)
                    {
                        GestureRecognized(this, new EventArgs());
                    }

                    //reset counter
                    
                }
            }
            //if result has been negative
            else
            {
                //reset counter
                partCounter = 0;

                //check this frame again, maybe it might be the start of a new gesture:
                //DO NOT USE RECURSITION since this might lead into a stack overflow!
                result = parts[this.partCounter].CheckGesturePart(body);
                if (result == GesturePartResult.Succeded)
                {
                    partCounter++;

                    // it is not necessary to check the boundaries of the array since we know, that this array has more than 2 fields
                }
            }
            Debug.WriteLine(this.name + ":" +partCounter);
        }

    }
}
