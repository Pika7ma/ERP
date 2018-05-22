# LPLFW：League of People who Let Father Worry

## 父愁者联盟 成员

- 哈哈哈组：魏日升、江文慧、赖长飞
- 他们去开黑我随便起名字组：禚晨晨、李沛昂、王信、张爽
- 没有感情的杀手组：田韵声、马平川、杨睿涵、葛润东
- 熊出没组：吴安琪、郭馨怡、鲁政
- 头铁组：王晶、庞泽华、李蕴辉、蒋艺璇

## 项目结构

- 界面层 `Lplfw/UI`:：窗口及窗口间的跳转逻辑
- 业务逻辑层 `Lplfw/BLL`： 从UI中抽离的业务逻辑
- 数据访问层 `Lplfw/DAL`： EF模型和数据视图

## Git约定

1. 各组创建分支，完成后提交合并
2. commit描述中，用`[feature]`开头来表示新特性，`[fixed]`开头来表示修改bug或重构等等

## 代码风格

1. 字段（私有成员变量）：小驼峰
2. 属性（公共成员变量）：大驼峰
3. 局部变量：以`_`开头的小驼峰