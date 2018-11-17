using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClassProperties
{
    class Program
    {
        static void Main(string[] args)
        {
            Student s = new Student("george", 7);

            foreach (var f in s.Fields())
            {
                Console.WriteLine(f);
            }

            s["Name"].SetValue("Test");
            s["Grade"].SetValue((byte)10);
            Console.WriteLine(s["Name"]);
            Console.WriteLine(s["Grade"]);

            Console.ReadKey();
        }
    }
    class FieldWrapper<T>
    {
        private FieldInfo Field;
        private readonly T Object;
        public FieldWrapper(T that, FieldInfo field)
        {
            Object = that;
            Field = field;
        }
        public object GetValue()
        {
            return Field.GetValue(Object);
        }
        public void SetValue(object value)
        {
            Field.SetValue(Object, value);
        }
        public object GetName()
        {
            return Field.Name;
        }

        public override string ToString()
        {
            return $"Property '{GetName()}' is type of '{Field.Name}' with value '{GetValue()}'.";
        }
    }
    class Student
    {
        public string Name;// { get; set; } Προσοχή αν το αλλαξεις σε properties τοτε δεν παιζει
        public byte Grade;// { get; set; } Προσοχή αν το αλλαξεις σε properties τοτε δεν παιζει
        public Student(string name, byte grade)
        {
            Name = name;
            Grade = grade;
        }
        public IEnumerable<FieldWrapper<Student>> Fields()
        {

            foreach (FieldInfo field in this.GetType().GetFields())
            {
                yield return new FieldWrapper<Student>(this, field);
            }
        }
        public FieldWrapper<Student> this[string fieldName]
        {
            get
            {
                return this.GetType().GetFields()
                    .Where(field => field.Name == fieldName)
                    .Select(field => new FieldWrapper<Student>(this, field))
                    .First();
            }
        }
    }
}
