using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Spectra_Splitter
{
    public class SpectralExperiment
    {

        //Initilize Some Variables
        List<string> _sampleIDs;
        int _numberOfMeasurementsPerSample;
        int _numberOfWavelengthsMeasured;
        int _numberofsamples;
        string _filename;
        List<SpectraRow> _spectraRowList;
        List<SampleRow> _sampleRowList;
        public List<SpectraRow> spectraRowList { get { return _spectraRowList; } }
        public List<SampleRow> sampleRowList { get { return _sampleRowList; } }
        public List<string> sampleIDs { get { return _sampleIDs; } }
        public int numberOfMeasurementsPerSample { get { return _numberOfMeasurementsPerSample; } }
        public int numberOfWavelengthsMeasured;
        //Ctor
        public SpectralExperiment(string filename, int numberofsamples)
        {
            _numberofsamples = numberofsamples;
            _filename = filename;
            _sampleIDs = getSampleIDs(filename, numberofsamples);
            _numberOfMeasurementsPerSample = getNumberOfExperiments(filename, numberofsamples);
            _spectraRowList = populateSpectraRows(filename, numberofsamples, numberOfMeasurementsPerSample);
            _numberOfWavelengthsMeasured = getWavelengthsMeasured(filename, numberofsamples);
            _sampleRowList = populateSampleRows(sampleIDs, spectraRowList, numberOfWavelengthsMeasured);
        }

        //Open the file and parse some things quick to get some information about what is contained in the file
        private static List<string> getSampleIDs(string filename, int samplenumber)
        {
            String line;
            List<string> tempsampleIDs = new List<string>();
            List<string> sampleIDs = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            line = file.ReadLine();
            file.Close();
            tempsampleIDs = line.Split(',').ToList<string>();
            for (int i = 1; i < samplenumber * 2; i += 2)
            {
                sampleIDs.Add(tempsampleIDs.ElementAt<string>(i).Split('_')[0]);
            }
            return sampleIDs;
        }
        private static int getNumberOfExperiments(string filename, int samplenumber)
        {
            String line;
            int numberofexperiments;
            List<string> columns = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            file.ReadLine();
            line = file.ReadLine();
            file.Close();
            columns = line.Split(',').ToList<string>();
            if ((float)(columns.Count() / (2 * samplenumber)) % 1 == 0)
            {
                numberofexperiments = columns.Count() / (2 * samplenumber);
                return numberofexperiments;
            }
            else
            {
                throw new System.Exception("Not all Samples and Experiments were included, did u forget to copy all of the data over?");
            }
        }
        private static int getWavelengthsMeasured(string filename, int samplenumber)
        {
            String line;
            int counter = 0;
            List<string> columns = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            file.ReadLine();
            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                counter++;
            }
            file.Close();
            return counter;
        }
        //Populate the internal data structures
        private static List<SpectraRow> populateSpectraRows(string filename, int samplenumber, int measurementspersample)
        {
            List<SpectraRow> spectraRowList = new List<SpectraRow>();
            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            string line;
            file.ReadLine();
            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                SpectraRow srow = new SpectraRow();
                String[] csvrow = line.Split(',');
                srow.wavelength = float.Parse(csvrow[0]);
                srow.samples = samplenumber;
                for (int i = 1; i < csvrow.Count(); i += 2)
                {
                    srow.absorbances.Add(float.Parse(csvrow[i]));
                }
                spectraRowList.Add(srow);
            }
            return spectraRowList;
        }
        private static List<SampleRow> populateSampleRows(List<String> sampleIDs, List<SpectraRow> spectra, int measurementspersample)
        {
            List<SampleRow> sampleRowList = new List<SampleRow>();
            for (int i = 0; i < sampleIDs.Count(); i++)
            {
                int totalsamples = sampleIDs.Count();
                for (int j = 0; j < spectra.Count(); j++)
                {
                    SampleRow sr = new SampleRow();
                    sr.sampleID = sampleIDs[i];

                    sr.wavelength = spectra[j].wavelength;
                    int sampletoextract = i;
                    for (int k = 0; k < spectra[j].absorbances.Count(); k += totalsamples)
                    {

                        sr.absorbances.Add(spectra[j].absorbances.ElementAt<float>(sampletoextract + k));
                    }
                    sampleRowList.Add(sr);
                }
            }
            return sampleRowList;
        }

        // Method that splits the CSV file into new file
        public void splitCSV()
        {
            for (int i = 1; i < _numberofsamples + 1; i++)
            {
                string samplename = "Sample" + i.ToString();
                string filenametowrite = _filename + "_" + samplename + ".csv";
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filenametowrite))
                {
                    sw.WriteLine(samplename);
                    foreach (SampleRow sr in _sampleRowList)
                    {
                        if (sr.sampleID == samplename)
                        {
                            sw.Write(sr.wavelength.ToString() + ",");
                            foreach (float fl in sr.absorbances)
                            {
                                string s = fl.ToString();
                                sw.Write(s + ",");
                            }
                            sw.WriteLine();
                        }
                    }
                }

            }
        }
    }
}
