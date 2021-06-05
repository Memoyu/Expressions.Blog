## 理解表达式树

表达式树以树形数据结构表示代码，其中每一个节点都是一种表达式(例如：x < y)，它将我们原来可以直接由代码编写的逻辑以表达式的方式存储在树状的结构里，从而可以在运行时去解析这个树，然后执行，实现动态的编辑和执行代码。

## 继承关系

Object -> Expression -> LambdaExpression -> Expression\<TDelegate>

 **Expression\<TDelegate>类**：将传入 的lambda 表达式与变量、字段或参数编译成表达式树。

**LambdaExpression类**：声明表达式。表达式树主要包含如下信息：

* 参数[Parameters]
* 表达式树类型[NodeType]
* 表达式[Body]
* 返回类型[ReturnType]
* Lambda表达式的委托[Compile]
* Lambda表达式名称[name]

**Expression **：表达式基类

## 常用表达式
| Expression | Desc |
| ---- | ---- |
|ConstantExpression|常量表达式|
|ParameterExpression|参数表达式|
|UnaryExpression|一元运算符表达式|
|BinaryExpression|二元运算符表达式|
|TypeBinaryExpression|is运算符表达式|
|ConditionalExpression|条件表达式|
|MemberExpression|访问字段或属性表达式|
|MethodCallExpression|调用成员函数表达式|
|Expression\<TDelegate>|委托表达式|

## 常用表达式节点类型
| NodeType | Desc |
| ---- | ---- |
|ExpressionType.And| C#中类似于&|
| ExpressionType.AndAlso| C#中类似于&& |
| ExpressionType.Or| C#中类似于|
| ExpressionType.OrElse| C#中类似于|
| ExpressionType.Equal| C#中类似于==|
| ExpressionType.NotEqual| C#中类似于!=|
| ExpressionType.GreaterThan| C#中类似于>|
| ExpressionType.GreaterThanOrEqual| C#中类似于>=|
| ExpressionType.LessThan| C#中类似于<|
| ExpressionType.LessThanOrEqual| C#中类似于<=|
| ExpressionType.Add| C#中类似于+|
| ExpressionType.AddChecked| C#中类似于+，进行溢出检查|
| ExpressionType.Subtract| C#中类似于-|
| ExpressionType.SubtractChecked| C#中类似于-，进行溢出检查|
| ExpressionType.Divide| C#中类似于/|
| ExpressionType.Multiply| C#中类似于*|
| ExpressionType.MultiplyChecked| C#中类似于*，进行溢出检查|

