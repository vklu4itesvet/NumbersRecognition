using NumbersRecognizer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NumbersRecognizer.Core
{
  public class DigitsFactory : IDigitsFactory
  {
    public IEnumerable<IDigit> CreateDigits()
    {
      return Assembly.GetAssembly(typeof(DigitBase))
        .GetTypes()
        .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(DigitBase)))
        .Select(t => (DigitBase)Activator.CreateInstance(t))
        .ToList();
    }
  }
}
