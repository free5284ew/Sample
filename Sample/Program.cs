using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Sample
{
    class Program
    {
        static void Main(string [] args)
        {
            Console.BufferHeight = short.MaxValue - 1;
            var r = Console.ReadLine(); //Add
            int x = 300, y = 100;

            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(q => q.GetTypes());
            types = types.Where(q => q.GetInterfaces().Any(c => c.Equals(typeof(ILambdaCreator))) & !q.IsAbstract);
            foreach (var type in types)
            {
                var obj = type.GetConstructor(Type.EmptyTypes).Invoke(null) as ILambdaCreator;
                if(obj.LambdaName == r)
                {
                    Console.WriteLine($"{x} {obj.LambdaName} {y} = {obj.Lambda(x, y)}");
                }
            }

            Console.ReadKey();
        }
    }
    


    public interface ILambdaCreator
    {
        string LambdaName { get; }
        T Lambda<T>(T t1, T t2) where T : IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>, new();
    }


    public abstract class FunctionFactory : ILambdaCreator 
    {
        protected FunctionFactory()
        {
        }

        public string LambdaName => this.GetType().Name;

        public virtual T Lambda<T>(T t1, T t2) where T : IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>, new()
        {
            switch (t1.GetTypeCode())
            {

                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.String:
                case TypeCode.Decimal:

                    return InnerOp(t1, t2);
            }
            return default(T);
        }

        protected abstract dynamic InnerOp(dynamic o1, dynamic o2);
    }

    public class Add : FunctionFactory
    {
        protected override dynamic InnerOp(dynamic o1, dynamic o2)
            => o1 + o2;
    }

    public class Sub : FunctionFactory
    {
        protected override dynamic InnerOp(dynamic o1, dynamic o2)
            => o1 - o2;
    }

    public class Mul : FunctionFactory
    {
        protected override dynamic InnerOp(dynamic o1, dynamic o2)
            => o1 * o2;
    }

    public class Div : FunctionFactory
    {
        protected override dynamic InnerOp(dynamic o1, dynamic o2)
            => o1 / o2;
    }


    public class Triangle : FunctionFactory
    {
        protected override dynamic InnerOp(dynamic o1, dynamic o2)
            => o1 * o2 / 2;
    }
}
