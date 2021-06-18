using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace _1_初探表达式
{
    class Program
    {
        static void Main(string[] args)
        {
            ParameterExpression numParam = Expression.Parameter(typeof(int), "num");//参数表达式
            ConstantExpression five = Expression.Constant(5, typeof(int));// 常量表达式
            BinaryExpression numLessThanFive = Expression.LessThan(numParam, five);//二元表达式（左右节点，例如 num < 5 ）
            Expression<Func<int, bool>> lambda1 = Expression.Lambda<Func<int, bool>>(
                numLessThanFive,
                new ParameterExpression[] { numParam });//构建lambda表达式
            Console.WriteLine($"表达式：{lambda1.ToString()}");
            var result = lambda1.Compile().Invoke(4);//编译表达式，执行lambda,传入参数num = 4
            Console.WriteLine($"表达式运行结果：{result}");

            var list = new List<long> { 10, 20, 30, 40, 50, 60 };
            list.Select((l, i) => 
            {
                Console.WriteLine($"第{i}个-{l}");
                return l;
            }).ToList();

            var datas = new List<ScoreClass>();
            datas.Add(new ScoreClass
            {
                CourseName = "数学",
                StudentName = "学生A",
                Score = 60
            });
            datas.Add(new ScoreClass
            {
                CourseName = "数学",
                StudentName = "学生B",
                Score = 65
            });
            datas.Add(new ScoreClass
            {
                CourseName = "数学",
                StudentName = "学生C",
                Score = 70
            });
            datas.Add(new ScoreClass
            {
                CourseName = "数学",
                StudentName = "学生D",
                Score = 75
            });
            datas.Add(new ScoreClass
            {
                CourseName = "数学",
                StudentName = "学生E",
                Score = 80
            });
            datas.Add(new ScoreClass
            {
                CourseName = "数学",
                StudentName = "学生F",
                Score = 81
            });
            datas.Add(new ScoreClass
            {
                CourseName = "数学",
                StudentName = "学生G",
                Score = 82
            });
            datas.Add(new ScoreClass
            {
                CourseName = "数学",
                StudentName = "学生H",
                Score = 83
            });
            datas.Add(new ScoreClass
            {
                CourseName = "数学",
                StudentName = "学生I",
                Score = 84
            });

            var request = new ScoreRequest()
            {
                CourseName = "数",
                StudentName = "H"
            };

            var expression = LambdaExtension.True<ScoreClass>();
            if (!string.IsNullOrWhiteSpace(request.CourseName))
                expression = expression.And(e => e.CourseName.Contains(request.CourseName));
            if (!string.IsNullOrWhiteSpace(request.StudentName))
                expression = expression.And(et => et.StudentName.Contains(request.StudentName));

            var resultDatas = datas.Where(expression.Compile())
                .ToList();
            Console.WriteLine($"查询结果：\n{string.Join("\n", resultDatas.Select(e => $"{e.StudentName} {e.CourseName} {e.Score}"))}");

        }

    }

    public class ScoreClass
    {
        public string CourseName { get; set; }
        public string StudentName { get; set; }
        public decimal Score { get; set; }
    }
}
