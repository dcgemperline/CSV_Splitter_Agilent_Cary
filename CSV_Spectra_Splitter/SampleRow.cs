using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Spectra_Splitter
{
    public class SampleRow
    {
        public string sampleID { get; set; }
        public float wavelength { get; set; }
        public List<float> absorbances = new List<float>();
    }
}
