using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

namespace Commercials
{
    public class Slot
    {
        [DataMember(Name = "commercial")]
        public string commercial { get; set; }

        [DataMember(Name = "start")]
        public string start { get; set; }

        [DataMember(Name = "duration")]
        public int duration { get; set; }
    }

    public class Timeline
    {
        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "defaultDuration")]
        public int defaultDuration { get; set; }

        [DataMember(Name = "slots")]
        public List<Slot> slots { get; set; }
    }
}
