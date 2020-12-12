using NumbersRecognizer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NumbersRecognizer.Core
{
  public class NumbersFactory : INumbersFactory
  {
    public IEnumerable<INumber> CreateNumbers()
    {
      return Assembly.GetAssembly(typeof(NumberBase))
        .GetTypes()
        .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(NumberBase)))
        .Select(t => (NumberBase)Activator.CreateInstance(t))
        .ToList();
    }
  }
}
