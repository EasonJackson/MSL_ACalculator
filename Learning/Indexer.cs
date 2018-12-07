using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning
{
    class Indexer
    {
        private string[] _DataContainer;

        public Indexer(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("Size cannot be smaller than 0");
            }
            _DataContainer = new string[size];
            for (int i = 0; i < _DataContainer.Length; i++)
            {
                _DataContainer[i] = "";
            }
        }

        public string this[int index]
        {
            get
            {
                return _DataContainer[index];
            }
            set
            {
                _DataContainer[index] = value;
            }
        }

        public string this[string key]
        {
            get
            {
                foreach (string element in _DataContainer)
                {
                    if (key == element)
                    {
                        return element;
                    }
                }
                return "";
            }

            set
            {
                for (int i = 0; i < _DataContainer.Length; i++)
                {
                    if (key == _DataContainer[i])
                    {
                        _DataContainer[i] = value;
                    }
                }
            }
        }
    }
}
