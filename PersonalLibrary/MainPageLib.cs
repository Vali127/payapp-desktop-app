using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using PayApp.ViewModels;

namespace PayApp.PersonalLibrary;

public class MainPageLib
{
    public static Dictionary<string, ViewModelBase> GetPages()
    {
        var pages = new Dictionary<string, ViewModelBase>
        {
            { "Empty", new EmptyPageViewModel() },
            { "Home", new HomePageViewModel() },
            { "Org", new OrgPageViewModel() },
        };
        
        return pages;
    }
}