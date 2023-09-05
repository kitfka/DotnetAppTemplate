using DotnetAppTemplate.Model;
using DotnetAppTemplate.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAppTemplate.Maui.Model;
public class FakeNavigationService : INavigationService
{
    public void Confirm(Action action, string message)
    {
        throw new NotImplementedException();
    }

    public void Confirm(Action action, string message, ConfirmWindowOptions options)
    {
        throw new NotImplementedException();
    }

    TView INavigationService.CreateWindow<TView>()
    {
        throw new NotImplementedException();
    }

    bool? INavigationService.ShowDialog<TView>()
    {
        throw new NotImplementedException();
    }

    void INavigationService.ShowView<TView>()
    {
        throw new NotImplementedException();
    }
}
