using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetAppTemplate.Model;
using DotnetAppTemplate.Model.Config;

namespace DotnetAppTemplate.ViewModel;

public class ConfigViewModel : ViewModelBase
{
    private Config Config;
    public ConfigViewModel(Config config) 
    {
        Config = config;
    }


    private bool testBool = Config.TestBool; 

    public bool TestBool
    {
        get { return testBool; }
        set 
        {
            testBool = value;
            Config.TestBool = value;
            NotifyPropertyChanged();
        }
    }

    private string testString = Config.TestString;

    public string TestString
    {   
        get { return testString; }
        set 
        { 
            testString = value;
            Config.TestString = value;
            NotifyPropertyChanged();
        }
    }
    


}
