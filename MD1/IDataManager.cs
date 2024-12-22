using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD1
{
    public interface IDataManager
    {
        //Metode print
        string print();

        //Metode save
        void save(string filePath);

        //Metode load
        void load(string filePath);

        //Metode createTestData
        void createTestData();

        //Metode reset
        void reset();
    }
}
