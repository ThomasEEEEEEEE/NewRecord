using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRecord_Backend.Models
{
    public enum RecordImageID
    {
        SOCCER,
        BASKETBALL,
        BASEBALL,
        FOOTBALL,
        RUNNING,
        CYCLING,
        LIFTING,
        BOWLING
        //rest
    }
    public class RecImage
    {
        public RecImage(string url, RecordImageID name)
        {
            ImageUrl = url;
            ImageID = name;
        }
        public string ImageUrl;
        public RecordImageID ImageID;
    }
}
