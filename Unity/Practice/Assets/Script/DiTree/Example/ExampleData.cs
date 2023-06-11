using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiTreeGroup
{
    public class ExampleBaseData
    {
        public string BaseData = "Base";
    }

    public class ExampleDataA : ExampleBaseData
    {
        public string DataA = "DataA";
    }
    
    public class ExampleDataAA : ExampleDataA
    {
        public string DataAA = "DataAA";
    }
    
    public class ExampleDataB : ExampleBaseData
    {
        public string DataB = "DataB";
    }
}