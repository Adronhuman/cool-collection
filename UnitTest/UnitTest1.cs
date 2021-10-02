using System;
using System.Collections.Generic;
using System.Linq;
using CoolCollection.CoolList;
using Xunit;

namespace UnitTest
{
    public class UnitTest1
    {
        private Type _collectionGenericType = typeof(CoolList<>);

        private List<Type> _types = new List<Type>
        {
            typeof(byte), typeof(sbyte), typeof(ushort), typeof(uint), typeof(ulong), typeof(short),
            typeof(int), typeof(long), typeof(decimal), typeof(double), typeof(float)
        };

        [Fact]
        public void TestCreationAndInitializationForAllTypes()
        {
            TestMethodForAllTypes(nameof(TestCreationAndInitialization));
        }

        [Fact]
        public void TestEnumerationForAllTypes()
        {
            TestMethodForAllTypes(nameof(TestEnumeration));
        }

        [Fact]
        public void GeneralTest()
        {
            var coolList = new CoolList<int>() { 14, 22, 19, 88 };

            //test reverse indexing
            Assert.Equal(coolList[^1], coolList[-1]);
            coolList.Insert(3, 39);

            Assert.Equal(new[] { 14, 22, 19, 39, 88 }, coolList.ToArray());

            coolList.Remove(22);
            Assert.Equal(new[] { 14, 19, 39, 88 }, coolList.ToArray());

            coolList.RemoveAt(1);
            Assert.Equal(new[] { 14, 39, 88 }, coolList.ToArray());

            int i = 0;
            foreach (var x in coolList)
            {
                Assert.Equal(coolList[i], x);
                i++;
            }
            Assert.Equal(3, coolList.Count);

            Assert.Equal(coolList.Max(), coolList.MaxElement);

            coolList.Insert(2, 666);
            Assert.Equal(coolList.Max(), coolList.MaxElement);
            Assert.Equal(2, coolList.IndexOfMax);

        }

        private void TestMethodForAllTypes(string methodName)
        {
            foreach (var type in _types)
            {
                typeof(UnitTest1).GetMethod(methodName)?.MakeGenericMethod(type).Invoke(this, null);
            }
        }

        private void TestCreationAndInitialization<T>() where T : struct, IComparable<T>, IEquatable<T>
        {
            //Default initialization
            var x = new CoolList<T>();

            Assert.Equal(0, x.Count);
            Assert.Throws<IndexOutOfRangeException>(() => (x[-1], x[0], x[1]));

            //initialization with specified capacity
            x = new CoolList<T>(14);
            Assert.Equal(0, x.Count);
            Assert.Throws<IndexOutOfRangeException>(() => (x[-1], x[15]));

            //initialization with parameters(this part also testing Add() method)
            x = new CoolList<T>() { new T(), new T(), new T()};
            Assert.Equal(3, x.Count);
            Assert.Equal(new List<T>(){new T(), new T(), new T()}, new List<T>{ x[0], x[1], x[2] });

            //insert at index
            x.Insert(1, new T());
            Assert.Equal(4, x.Count);
            Assert.Equal(new T(), x[2]);

            //remove value from list
            x.Remove(new T());
            Assert.Equal(3, x.Count);
        }

        private void TestEnumeration<T>() where T : struct, IComparable<T>, IEquatable<T>
        {
            var x = new CoolList<T>() { new T(), new T(), new T() };

            var i = 0;
            foreach (var element in x)
            {
                Assert.Equal(new T(), element);
                i++;
            }

            Assert.Equal(x.Count, i);
        }

        private void TestMaxElement() { }
    }
}
