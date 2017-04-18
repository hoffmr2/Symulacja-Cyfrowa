using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomGenerators;

namespace WirelessNetworkComponents
{
    public class RandomGeneratorsAnalyzer
    {
        public const int MaxSeedSetIndex = 30;
        private double _lambda;
        private int _unifromGeneratorUpBound;
        private int _uniformGeneratorDownBound;
        private int _samplesNumber;
        private int _seedSet;
        private SortedDictionary<double, int> _uniformGeneratorHistogram;
        private SortedDictionary<double, int> _expGeneratorHistogram;
        private UniformRandomGenerator _uniformRandomGenerator;
        private ExponentialRandomGenerator _exponentialRandomGenerator;

        public RandomGeneratorsAnalyzer()
        {
            _uniformGeneratorHistogram = new SortedDictionary<double, int>();
            _expGeneratorHistogram = new SortedDictionary<double, int>();
        }

        public double Lambda
        {
            get { return _lambda; }
            set { _lambda = value; }
        }

        public int UnifromGeneratorUpBound
        {
            get { return _unifromGeneratorUpBound; }
            set { _unifromGeneratorUpBound = value; }
        }

        public int UniformGeneratorDownBound
        {
            get { return _uniformGeneratorDownBound; }
            set { _uniformGeneratorDownBound = value; }
        }

        public int SamplesNumber
        {
            get { return _samplesNumber; }
            set { _samplesNumber = value; }
        }

        public int SeedSet
        {
            get { return _seedSet; }
            set { _seedSet = value; }
        }

        public SortedDictionary<double, int> UniformGeneratorHistogram
        {
            get { return _uniformGeneratorHistogram; }
            set { _uniformGeneratorHistogram = value; }
        }

        public SortedDictionary<double, int> ExpGeneratorHistogram
        {
            get { return _expGeneratorHistogram; }
            set { _expGeneratorHistogram = value; }
        }

        private void InitGenerators(int seedSet, double lambda)
        {
            var file = new StreamReader("seeds.txt");
            var line = file.ReadToEnd();
            var lines = line.Split('\n');
            var expectedLine = lines[seedSet];
            var seeds = expectedLine.Split(':');

            try
            {
                _uniformRandomGenerator = null;
                _exponentialRandomGenerator = null;
                _uniformRandomGenerator = new UniformRandomGenerator(int.Parse(seeds[0]));
                _exponentialRandomGenerator = new ExponentialRandomGenerator(lambda, int.Parse(seeds[1]));
            }
            catch
            {
                throw new Exception("Error initialising random Generators");
            }

        }

        public bool IsInitialized()
        {
            if (SamplesNumber <= 0)
                return false;
            if (Lambda <= 0)
                return false;
            if (_seedSet <= 0 && _seedSet > MaxSeedSetIndex)
                return false;
            if (UnifromGeneratorUpBound < UniformGeneratorDownBound)
                return false;
            if (UniformGeneratorDownBound < 0)
                return false;
            if (UnifromGeneratorUpBound < 0)
                return false;
            return true;
        }

        public void RunAnalysis()
        {
            if (IsInitialized() == false)
                return;
            else
            {
                InitGenerators(SeedSet,Lambda);
                _uniformGeneratorHistogram.Clear();
                _expGeneratorHistogram.Clear();

                for (int i = 0; i < SamplesNumber; ++i)
                {
                    var randomNumber =  Math.Floor( _uniformRandomGenerator.Rand(UniformGeneratorDownBound, UnifromGeneratorUpBound));
                    AddToUniformHistogram(randomNumber);

                    randomNumber = _exponentialRandomGenerator.Rand();
                    randomNumber *= 10;
                    randomNumber = Math.Floor(randomNumber);
                    randomNumber /= 10;
                    AddToExpHistogram(randomNumber);
                }
            }
        }

        private void AddToExpHistogram(double randomNumber)
        {
            if (_expGeneratorHistogram.ContainsKey(randomNumber))
                ++_expGeneratorHistogram[randomNumber];
            else
            {
                _expGeneratorHistogram.Add(randomNumber, 1);
            }
        }

        private void AddToUniformHistogram(double randomNumber)
        {
            if (_uniformGeneratorHistogram.ContainsKey(randomNumber))
                ++_uniformGeneratorHistogram[randomNumber];
            else
            {
                _uniformGeneratorHistogram.Add(randomNumber, 1);
            }
        }
    }
}
