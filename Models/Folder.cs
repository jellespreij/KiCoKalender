using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public enum Folder
    {
        Picture = 1,
        Medical = 2,
        Finance = 3,
        School = 4,
        Other = 5
    }
}
