using ControllerRebinder.Common.Moddels;
using ControllerRebinder.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerRebinder.Core.Helpers
{
    public static class QuadrantHelper
    {
        public static Quadrant WhereAmI(int LeftStickX, int LeftStickY)
        {
            if(LeftStickX <= 0 && LeftStickY > 0)
            {
                return Quadrant.TopLeft;
            }
            else if(LeftStickX > 0 && LeftStickY > 0)
            {
                return Quadrant.TopRight;
            }
            else if(LeftStickX <= 0 && LeftStickY < 0)
            {
                return Quadrant.BottomLeft;
            }
            else if(LeftStickX > 0 && LeftStickY <= 0)
            {
                return Quadrant.BottomRight;
            }

            return Quadrant.DefaultQuadrant;
        }


        public static ZoneRange WhereAmI(List<ZoneRange> ranges, double _currentXArea)
        {
            foreach(var range in ranges)
            {
                if(range.ZoneCalculationType == ZoneCalctulationType.Positive)
                {
                    if(_currentXArea > range.Left)
                    {
                        return range;
                    }
                }
                else if(range.ZoneCalculationType == ZoneCalctulationType.Between)
                {
                    if(_currentXArea > range.SmallerNumber() && _currentXArea < range.BiggerNumber())
                    {
                        return range;
                    }
                }
                else if(range.ZoneCalculationType == ZoneCalctulationType.Negative)
                {
                    if(_currentXArea < range.Left)
                    {
                        return range;
                    }
                }
            }

            return null;
            ;
        }
    }
}
