using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Compatibility;
using PhoneNumbers;

namespace PhoneValidation
{
    internal class Tests
    {
        private const string NumberToParse = "+18185555774";
        private PhoneNumberUtil _phoneNumberUtil;

        [OneTimeSetUp]
        public void SetUp()
        {
            _phoneNumberUtil = PhoneNumberUtil.GetInstance();
        }

        [Test]
        public void Should18()
        {
            var isValidNumber = IsValidNumber(NumberToParse);
            Assert.That(isValidNumber, Is.True);
        }

        private bool IsValidNumber(string phoneNumber1)
        {
            PhoneNumber parsedPhoneNumber;
            try { parsedPhoneNumber = _phoneNumberUtil.Parse(phoneNumber1, null); }
            catch
            {
                return false;
            }
            return _phoneNumberUtil.IsValidNumber(parsedPhoneNumber);
        }

        [Test]
        public void Should30()
        {
            var phoneNumber = _phoneNumberUtil.Parse(NumberToParse, null);
            var isValidNumber = _phoneNumberUtil.IsPossibleNumber(phoneNumber);
            Assert.That(isValidNumber, Is.True);
        }

        [Test]
        public void Should38()
        {
            var times = new List<long>();

            var lines = File.ReadAllLines("C:\\tmp\\badphonenumber-clean.log");
            foreach (var line in lines)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var isValidNumber = IsValidNumber(line.Trim());
                stopwatch.Stop();
                if (isValidNumber)
                {
                    Console.WriteLine("{0}", line);
                }
                times.Add(stopwatch.ElapsedMilliseconds);
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
            }

            var average = times.Average();
            var count = times.Count;
            var sum = times.Sum();
            var max = times.Max();
            Console.WriteLine("{0} ms. {1} numbers. {2} sum. {3} max", average, count, sum, max);
        }
    }
}