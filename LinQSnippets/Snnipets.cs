using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LinQSnippets
{


    public class Snnipets
    {
        static public void BasicLinQ()
        {
            string[] cars =
            {
                "VW Golf",
                "VW California",
                "Audi A3",
                "Audi A5",
                "Fiat Punto",
                "Seat Ibiza",
                "Sear Leon"
            };

            //1. Select * of cars
            var carList = from car in cars select car;

            foreach ( var car in carList )
            {
                Console.WriteLine(car);
            }
            //2. Select Where car is Audi
            var audiList = from car in cars where car.Contains("Audi") select car;

            foreach (var audi in audiList)
            {
                Console.WriteLine(audi);
            }

        }
        //Number Examples
        static public void NumbersLinQ()
        {
            List<int> numbers = new List<int> { 1,2,3,4,5,6,7,8,9 };
            //Each Number multiplied by 3 
            //Take all numbers, bu 9 no
            // Order numbers by Ascending value
            var processedNumberList = numbers
                .Select(num => num * 3)//(3,6,9, etc)
                .Where(num => num != 9)//(All but no 9)
                .OrderBy(num => num);//(At the end, order ascending)
        }

        //Search advanced
        static public void SearchExamples()
        {
            List<string> textlist = new List<string> 
            {
                "a",
                "bx",
                "c",
                "d",
                "e",
                "cj",
                "f",
                "c"
            };
            //1. Fist of all elements
            var First  = textlist.First();

            //2. First Element has "c"
            var cText = textlist.First(text => text.Equals("c"));

            //3. First Element that contains "j"
            var jtext = textlist.First(text => text.Contains("j"));

            //4. First Element that contains Z or default
            var zFirstText = textlist.FirstOrDefault(text => text.Contains("z")); //" " or first element that contains "z"

            //5. Last Element that contains Z or default
            var zLastText = textlist.LastOrDefault(text => text.Contains("z"));//" " or last element that contains "z"

            //6. Single Values
            var uniqueText = textlist.Single();
            var uniqueDefaultText = textlist.SingleOrDefault();

            int[] evenNumbers = { 0, 2, 4, 6, 8 };
            int[] othersEvenNumbers = { 0, 2, 6};
            //Obtain {4,8}
            var myEvenNumbers = evenNumbers.Except(othersEvenNumbers);//{4, 8}
        }

        static public void MultipleSelects()
        {
            //Select Many
            string[] myOpinions = {
                "Opinion 1, text 1",
                "Opinion 2, text 2",
                "Opinion 3, text 3"

            };

            var myOpinionSelection = myOpinions.SelectMany(opinion => opinion.Split(","));

            //Class with LinQ
            var enterprises = new[]
            {
                new Enterprise()
                {
                    Id = 1,
                    Name = "Enterprise 1",
                    Employees = new[]
                    {
                        new Employee
                        {
                            Id = 1,
                            Name = "Pedro",
                            Email = "pedro@gmail.com.mx",
                            Salary = 3000
                        },
                        new Employee
                        {
                            Id = 2,
                            Name = "Martin",
                            Email = "Martin@gmail.com.mx",
                            Salary = 1000
                        },
                        new Employee
                        {
                            Id = 3,
                            Name = "Pepe",
                            Email = "Pepe@gmail.com.mx",
                            Salary = 2000
                        }
                    }
                },
                new Enterprise()
                {
                    Id = 2,
                    Name = "Enterprise 2",
                    Employees = new[]
                    {
                        new Employee
                        {
                            Id = 4,
                            Name = "Ana",
                            Email = "ana@gmail.com.mx",
                            Salary = 3000
                        },
                        new Employee
                        {
                            Id = 5,
                            Name = "Maria",
                            Email = "maria@gmail.com.mx",
                            Salary = 1500
                        },
                        new Employee
                        {
                            Id = 6,
                            Name = "Martha",
                            Email = "martha@gmail.com.mx",
                            Salary = 4000
                        }
                    }
                }
            };

            // Obtain all Employees of All Enterprises
            var employesList = enterprises.SelectMany(enterprise => enterprise.Employees);

            // Know if an a list is empty
            bool hasEnterprise = enterprises.Any();

            bool hasEmployes = enterprises.Any(enterprise => enterprise.Employees.Any());

            // All Enterprises at least has an employees with more than $1000 of salary

            bool hasEmployeeWithSalaryMoreThanOrEqual1000 = enterprises.Any(enterprise => enterprise.Employees.Any(employee => employee.Salary >= 1000));

        }

        static public void CollectionsLinQ()
        {
            var firstList = new List<string>() { "a", "b", "c" };
            var secondList = new List<string>() { "a", "c", "d" };

            //Inner Join
            var commonResult = from element in firstList
                               join secondElement in secondList
                               on element equals secondElement
                               select new { element, secondElement };

            var commonResult2 = firstList.Join(secondList,
                                        element => element,
                                        secondElement => secondElement,
                                        (element,secondElement) => new {element, secondElement}
                                );
            //Outer Join - Left
            var leftOuterJoin = from element in firstList
                                join secondElement in secondList
                                on element equals secondElement//Take "Inner Join" from the tables
                                into temporalList//Save the "Inner Join" in a temporal List
                                from temporalElement in temporalList.DefaultIfEmpty()//Search into "Temporal list"
                                where element != temporalElement//If not equals element from temporarl list 
                                select new { Element = element };//Give me the new elements

            var leftOuterJoin2 = from element in firstList
                                 from secondElement in secondList.Where(s => s==element).DefaultIfEmpty()
                                 select new { Element = element, SecondElement = secondElement };

            //Outer Join - Right
            var rightOuterJoin = from secondElement in secondList
                                 join element in firstList
                                on secondElement equals element//Take "Inner Join" from the tables
                                into temporalList//Save the "Inner Join" in a temporal List
                                from temporalElement in temporalList.DefaultIfEmpty()//Search into "Temporal list"
                                where secondElement != temporalElement//If not equals element from temporarl list 
                                select new { Element = secondElement };//Give me the new elements

            //Union from tables
            var unionList = leftOuterJoin.Union(rightOuterJoin);
            
        }
        
        static public void SkipElementLinq()
        {
            var myList = new[]
            {
                1,2,3,4,5,6,7,8,9,10
            };
            //SKIP
            var skipTwoFirstElement = myList.Skip(2);//{3,4,5,6,7,8,9,10};

            var skipTwoLastElement = myList.SkipLast(2);//{1,2,3,4,5,6,7,8};

            //Skip Elements that are less 4
            var skipWhileSmaller = myList.SkipWhile(x => x < 4); //{5,6,7,8,9,10};

            //TAKE
            var takeFirstTwoElements = myList.Take(2);//{1,2};

            var takeLastTwoElements = myList.TakeLast(2);//{1,2};
            //Take Elements that are less 4
            var takeWhileSmaller = myList.TakeWhile(x => x < 4); //{1,2,3}
        }

        //paging with skip and take
        static public IEnumerable<T> GetPage<T>(IEnumerable<T> collection, int pageNumber, int resultPerPage)
        {
            int startIndex = (pageNumber - 1) * resultPerPage;
            return collection.Skip(startIndex).Take(resultPerPage);
        }
        
        //Variables
        static public void VariablesLinQ()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 8, 9, 10 };
            //Media
            var aboveAverage = from number in numbers//This is how declare variables in LinQ
                               let average = numbers.Average()//Average from Numbers
                               let nSquare = Math.Pow(number, 2)//number "al cuadrado"
                               where nSquare > average 
                               select number;

            Console.WriteLine("Average: {0}", numbers.Average());

            foreach ( int number in aboveAverage ) 
            {
                Console.WriteLine("Query: {0} Square{1}", number, Math.Pow(number,2));
            }
        }
        //ZIP
        static public void ZipLinQ()
        {
            int[] numbers = { 1, 2, 3, 4, 5 };
            string[] stringNumbers = { "one", "two", "three", "four", "five" };

            IEnumerable<string> zipNumbers = numbers.Zip(stringNumbers, (number, word) => number + " = " + word);
            //{"1 = one","2 = two"}
        }

        //repeat and range
        static public void repeatRangeLinQ()
        {
            //Generate collector from 1 - 1000 RANGE
            // IEnumerable<int>
            var first1000 = Enumerable.Range(1, 1000);

            var aboveAverage = from number in first1000
                               select number;

            //Repeat a Values N Time
            //IEnumerable<string>
            var fiveXs = Enumerable.Repeat("X",5); //{"X","X","X","X","X"}
        }

        static public void studentsLinQ()
        {
            var classRoom = new[]
            {
                new Students
                {
                    Id = 1,
                    Name = "Pedro",
                    Grade = 90,
                    Certified = true,
                },
                new Students
                {
                    Id = 2,
                    Name = "Juan",
                    Grade = 50,
                    Certified = false,
                },
                new Students
                {
                    Id = 3,
                    Name = "Ana",
                    Grade = 96,
                    Certified = true,
                },
                new Students
                {
                    Id = 4,
                    Name = "Alvaro",
                    Grade = 10,
                    Certified = false,
                },
                new Students
                {
                    Id = 5,
                    Name = "Martin",
                    Grade = 50,
                    Certified = true,
                }
            };
            //Select students that they are certified
            var cetifiedStudents = from student in classRoom
                                   where student.Certified == true
                                   select student;

            //Select students that they are not certified
            var notCertifiedStudents = from student in classRoom
                                       where student.Certified == false
                                       select student;

            //Select aproved students
            var aprovedStudents = from student in classRoom
                                  where student.Grade >= 50 && student.Certified == true
                                  select student.Name;//student.Grade
        }

        //ALL
        static public void AllLinQ()
        {
            var numbers = new List<int>() {1,2,3,4,5 };

            bool allAreSmallerThan10 = numbers.All(x => x < 10); //true

            bool allAreBiggerOrEqualThan2 = numbers.All(x => x > 2); //false

            var emptyList = new List<int>();
            
            bool allNumbersAreGreaterThan0 = emptyList.All(x => x > 0);//true
        }

        //Aggregate
        static public void aggregateQueries()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //Sum All Numbers
            int sum = numbers.Aggregate((prevSum, current) => prevSum + current);
            // 0,1 => 1
            // 1,2 => 3
            // 3,4 => 7
            //etc.


            string[] words = { "Hello","my","name", "is", "Pedro" };
            string greeting = words.Aggregate((prevGreeting, current) => prevGreeting + current);
            // "", "Hello" => Hello
            // "Hello" , "my" => Hello, my
            // "Hello" , "my" , "Name" => Hello my name
        }

        //Disctinct
        static public void distinctValues()
        {
            int[] numbers = { 1,2,3,4,5,5,4,3,2,1};

            IEnumerable<int> disctinctValues = numbers.Distinct();
        }


        //GroupBy
        static public void groupByLinQ()
        {
            List<int> numbers = new List<int>() {1,2,3,4,5,6,7,8,9 };

            //Obtain only even numbers and generate two groups
            var grouped = numbers.GroupBy(x => x % 2 == 0);
            //We will have two groups
            //1. The group that doesnt fit the condition (odd number)
            //2. the group that fits the condition (even numbers)

            foreach(var group in grouped)
            {
                foreach(var value in group)
                {
                    Console.WriteLine(value); // 1,3,5,7,9 ... 2,4,6,8 (First the odds and then the even)
                }
            }

            //Another example
            var classRoom = new[]
            {
                new Students
                {
                    Id = 1,
                    Name = "Pedro",
                    Grade = 90,
                    Certified = true,
                },
                new Students
                {
                    Id = 2,
                    Name = "Juan",
                    Grade = 50,
                    Certified = false,
                },
                new Students
                {
                    Id = 3,
                    Name = "Ana",
                    Grade = 96,
                    Certified = true,
                },
                new Students
                {
                    Id = 4,
                    Name = "Alvaro",
                    Grade = 10,
                    Certified = false,
                },
                new Students
                {
                    Id = 5,
                    Name = "Martin",
                    Grade = 50,
                    Certified = true,
                }
            };

            var certifiedQuery = classRoom.GroupBy(student => student.Certified && student.Grade >= 50);

            // We obtain two groups
            // 1- No certified students
            // 2- Certified Students
            foreach (var group in certifiedQuery)
            {
                Console.WriteLine("--- {0} ---", group.Key);
                foreach (var student in group)
                {
                    Console.WriteLine(student.Name);
                }
            }

        }

        static public void relationsLinQ()
        {
            List<Post> posts = new List<Post>() 
            {
                new Post()
                {
                    Id = 1,
                    Title = "My First Post",
                    Content = "My First Content",
                    Created = DateTime.Now,
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Id = 1,
                            Created = DateTime.Now,
                            Title = "My First Comment",
                            Content = "My Content"
                        },
                        new Comment()
                        {
                            Id = 2,
                            Created = DateTime.Now,
                            Title = "My Second Comment",
                            Content = "My Other Content"
                        }
                    }
                },
                new Post()
                {
                    Id = 2,
                    Title = "My Second Post",
                    Content = "My Second Content",
                    Created = DateTime.Now,
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Id = 3,
                            Created = DateTime.Now,
                            Title = "My Other Comment",
                            Content = "My New Content"
                        },
                        new Comment()
                        {
                            Id = 4,
                            Created = DateTime.Now,
                            Title = "My Other New Comment",
                            Content = "My new Other Content"
                        }
                    }
                }
            };

            var commentContent = posts.SelectMany(post => post.Comments,
                                                        (post, comment) => new { PostId = post.Id, comment.Content });

        }
    }
}