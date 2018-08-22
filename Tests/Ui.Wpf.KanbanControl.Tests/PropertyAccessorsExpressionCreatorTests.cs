using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using Ui.Wpf.KanbanControl.Expressions;

namespace Ui.Wpf.KanbanControl.Tests
{
    [TestFixture]
    public class PropertyAccessorsExpressionCreatorTests
    {
        [Test]
        public void PropertyAccessorsExpressionCreator_Should_Make_Accessors_To_Any_Property()
        {
            var pa = new PropertyAccessorsExpressionCreator(typeof(MockClass));

            var aGetter = pa.TakeGetterForProperty("A");
            var bGetter = pa.TakeGetterForProperty("B");
            var subGetter = pa.TakeGetterForProperty("SubClass");
            var subParentGetter = pa.TakeGetterForProperty("SubClass.Parent");
            var subAnotherGetter = pa.TakeGetterForProperty("SubClass.AnotherParent");
            var subAGetter = pa.TakeGetterForProperty("SubClass.A");
            var subBGetter = pa.TakeGetterForProperty("SubClass.B");

            Assert.NotNull(aGetter);
            Assert.NotNull(bGetter);
            Assert.NotNull(subGetter);
            Assert.NotNull(subParentGetter);
            Assert.NotNull(subAnotherGetter);
            Assert.NotNull(subAGetter);
            Assert.NotNull(subBGetter);
        }

        [Test]
        public void PropertyAccessorsExpressionCreator_Should_Correctly_Get_And_Set()
        {
            var pa = new PropertyAccessorsExpressionCreator(typeof(MockClass));

            var initialA = 333;
            var initialB = "444";
            var initialSubA = 777;
            var initialSubB = "888";

            var obj = new MockClass { A = initialA, B = initialB };
            var sub = new SubClass { A = initialSubA, B = initialSubB, Parent = obj, AnotherParent = null };
            obj.SubClass = sub;

            var aGetter = pa.TakeGetterForProperty("A");
            var bGetter = pa.TakeGetterForProperty("B");
            var subAGetter = pa.TakeGetterForProperty("SubClass.A");
            var subBGetter = pa.TakeGetterForProperty("SubClass.B");

            var subASetter = pa.TakeSetterForProperty("SubClass.A");
            var subBSetter = pa.TakeSetterForProperty("SubClass.B");


            Assert.NotNull(subAGetter);
            Assert.NotNull(subBGetter);
            Assert.NotNull(subASetter);
            Assert.NotNull(subBSetter);


            Assert.AreEqual(initialA, aGetter(obj));
            Assert.AreEqual(initialB, bGetter(obj));
            Assert.AreEqual(initialSubA, subAGetter(obj));
            Assert.AreEqual(initialSubB, subBGetter(obj));

            subASetter(obj, -333);

            Assert.AreEqual(-333, obj.SubClass.A);

        }

        [Test]
        public void PropertyAccessorsExpressionCreator_Must_Do_Null_Proposition()
        {
            var pa = new PropertyAccessorsExpressionCreator(typeof(MockClass));

            var obj = new MockClass();

            var aGetter = pa.TakeGetterForProperty("A");
            var subAGetter = pa.TakeGetterForProperty("SubClass.A");

            Assert.NotNull(subAGetter);

            Assert.DoesNotThrow(() => aGetter(null));
            Assert.DoesNotThrow(() => aGetter(obj));

            Assert.DoesNotThrow(() => subAGetter(null));
            Assert.DoesNotThrow(() => subAGetter(obj));

            Assert.AreEqual(default(int), subAGetter(obj));
        }

        [Test]
        public void PropertyAccessorsExpressionCreator_Should_Not_Be_To_Slow()
        {
            var pa = new PropertyAccessorsExpressionCreator(typeof(MockClass));

            var testObjects = Enumerable
                .Range(0, 10000)
                .Select(x =>
                {
                    var obj = new MockClass { A = x, B = x.ToString() };

                    var sub = new SubClass { A = x * 2, B = (x * 2).ToString(), Parent = obj, AnotherParent = null };

                    obj.SubClass = sub;

                    return obj;
                })
                .ToArray();


            var swGenericProperty = Stopwatch.StartNew();
            int a = 0;
            string b = "";
            SubClass c = null;
            int? sa = 0;
            string sb = "";
            MockClass sp = null;
            MockClass sap = null;

            for (int i = 0; i < 2000; i++)
            {
                foreach (var obj in testObjects)
                {
                    a = obj.A;
                    b = obj.B;
                    c = obj.SubClass;
                    sa = obj?.SubClass?.A;
                    sb = obj?.SubClass?.B;
                    sp = obj?.SubClass?.Parent;
                    sap = obj?.SubClass?.AnotherParent;
                }
            }

            var z = a + sa.Value;
            var zz = b + sb;
            var zc = c;
            var zsp = sp;
            var zsap = sap;

            var genericPropertyElapsed = swGenericProperty.Elapsed;
            Console.WriteLine(genericPropertyElapsed);

            var swExpressionProperty = Stopwatch.StartNew();

            var aGetter = pa.TakeGetterForProperty("A");
            var bGetter = pa.TakeGetterForProperty("B");
            var subGetter = pa.TakeGetterForProperty("SubClass");
            var subParentGetter = pa.TakeGetterForProperty("SubClass.Parent");
            var subAnotherGetter = pa.TakeGetterForProperty("SubClass.AnotherParent");
            var subAGetter = pa.TakeGetterForProperty("SubClass.A");
            var subBGetter = pa.TakeGetterForProperty("SubClass.B");

            for (int j = 0; j < 2000; j++)
            {
                foreach (var obj in testObjects)
                {
                    a = (int)aGetter(obj);
                    b = (string)bGetter(obj);
                    c = (SubClass)subGetter(obj);
                    sp = (MockClass)subParentGetter(obj);
                    sap = (MockClass)subAnotherGetter(obj);
                    sa = (int)subAGetter(obj);
                    sb = (string)subBGetter(obj);
                }
            }
            var expressionPropertyElapsed = swExpressionProperty.Elapsed;
            Console.WriteLine(expressionPropertyElapsed);

            Assert.LessOrEqual(
                Math.Abs(genericPropertyElapsed.TotalMilliseconds - expressionPropertyElapsed.TotalMilliseconds),
                genericPropertyElapsed.TotalMilliseconds * 18);
        }

        public class MockClass
        {
            public int A { get; set; }

            public string B { get; set; }

            public SubClass SubClass { get; set; }
        }

        public class SubClass
        {
            public int A { get; set; }

            public string B { get; set; }

            public MockClass Parent { get; set; }

            public MockClass AnotherParent { get; set; }
    }
    }
}
