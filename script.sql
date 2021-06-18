USE [timetable_db]
GO
/****** Object:  UserDefinedFunction [dbo].[get_facultyid]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[get_facultyid](@specialty_id int)
RETURNS int
AS
BEGIN
RETURN (SELECT faculty_id FROM specialties WHERE @specialty_id = specialties.specialty_id)
END;
GO
/****** Object:  UserDefinedFunction [dbo].[get_getdepartmentid_fromdiscipline]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[get_getdepartmentid_fromdiscipline](@discipline_id int)
RETURNS int
AS
BEGIN
RETURN (SELECT department_id FROM disciplines WHERE @discipline_id = discipline_id)
END;
GO
/****** Object:  UserDefinedFunction [dbo].[get_getdepartmentid_fromteacher]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[get_getdepartmentid_fromteacher](@teacherid int)
RETURNS int
AS
BEGIN
RETURN (SELECT department_id FROM teachers WHERE teacher_id = @teacherid)
END;
GO
/****** Object:  UserDefinedFunction [dbo].[get_traintype_fromgroup]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[get_traintype_fromgroup](@group_id int)
RETURNS int
AS
BEGIN
RETURN (SELECT id_trainform FROM groups WHERE @group_id = groups.group_id)
END;
GO
/****** Object:  UserDefinedFunction [dbo].[timetable_hasValues]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[timetable_hasValues](@group_id int, @lesson_number int, @weekday_id int, @week_account int)
RETURNS int
AS
BEGIN
RETURN (SELECT COUNT(*) from timetable 
		where 
		timetable.group_id = @group_id 
		and timetable.lesson_number = @lesson_number
		and timetable.weekday_id = @weekday_id
		and timetable.week_account = @week_account)
END;
GO
/****** Object:  Table [dbo].[dir_lessons]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dir_lessons](
	[lesson_number] [int] NOT NULL,
	[start_time] [time](7) NOT NULL,
	[end_time] [time](7) NOT NULL,
 CONSTRAINT [PK_dir_lessons] PRIMARY KEY CLUSTERED 
(
	[lesson_number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[lesson_msg]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[lesson_msg]
AS
SELECT *, concat(lesson_number, ' пара (', CONVERT(VARCHAR, start_time, 20) , ' - ', CONVERT(VARCHAR, end_time, 20), ')') as [full_lesson] FROM dir_lessons
GO
/****** Object:  Table [dbo].[specialties]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[specialties](
	[specialty_id] [int] IDENTITY(1,1) NOT NULL,
	[specialty_name] [nvarchar](75) NOT NULL,
	[faculty_id] [int] NOT NULL,
 CONSTRAINT [PK_specialties] PRIMARY KEY CLUSTERED 
(
	[specialty_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[timetable]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[timetable](
	[timetable_id] [int] IDENTITY(1,1) NOT NULL,
	[group_id] [int] NOT NULL,
	[room] [int] NOT NULL,
	[teacher_id] [int] NOT NULL,
	[discipline_id] [int] NOT NULL,
	[lesson_number] [int] NOT NULL,
	[date] [date] NOT NULL,
	[evenweek] [bit] NOT NULL,
	[completed] [bit] NOT NULL,
 CONSTRAINT [PK_timetable_1] PRIMARY KEY CLUSTERED 
(
	[timetable_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[disciplines]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[disciplines](
	[discipline_id] [int] IDENTITY(1,1) NOT NULL,
	[discipline_name] [nvarchar](50) NOT NULL,
	[department_id] [int] NOT NULL,
 CONSTRAINT [PK_disciplines] PRIMARY KEY CLUSTERED 
(
	[discipline_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[teachers]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[teachers](
	[teacher_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](75) NOT NULL,
	[position_id] [int] NOT NULL,
	[rank_id] [int] NOT NULL,
	[department_id] [int] NOT NULL,
	[login] [varchar](20) NOT NULL,
	[passwd] [varchar](50) NOT NULL,
 CONSTRAINT [PK_teachers_1] PRIMARY KEY CLUSTERED 
(
	[teacher_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[groups]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[groups](
	[group_id] [int] NOT NULL,
	[count_people] [int] NOT NULL,
	[id_specialty] [int] NOT NULL,
	[id_trainform] [int] NOT NULL,
	[id_faculty] [int] NOT NULL,
 CONSTRAINT [PK_groups_1] PRIMARY KEY CLUSTERED 
(
	[group_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[faculties]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[faculties](
	[faculty_id] [int] IDENTITY(1,1) NOT NULL,
	[faculty_name] [nvarchar](75) NOT NULL,
 CONSTRAINT [PK_faculties] PRIMARY KEY CLUSTERED 
(
	[faculty_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[positions]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[positions](
	[position_id] [int] IDENTITY(1,1) NOT NULL,
	[position_name] [nvarchar](25) NOT NULL,
 CONSTRAINT [PK_positions] PRIMARY KEY CLUSTERED 
(
	[position_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[train_types]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[train_types](
	[traintype_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](25) NOT NULL,
 CONSTRAINT [PK_train_types] PRIMARY KEY CLUSTERED 
(
	[traintype_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[departments]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[departments](
	[department_id] [int] IDENTITY(1,1) NOT NULL,
	[department_name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_departments] PRIMARY KEY CLUSTERED 
(
	[department_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ranks]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ranks](
	[rank_id] [int] IDENTITY(1,1) NOT NULL,
	[rank_name] [nvarchar](75) NOT NULL,
 CONSTRAINT [PK_ranks] PRIMARY KEY CLUSTERED 
(
	[rank_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[audiences]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[audiences](
	[room] [int] NOT NULL,
	[capacity] [int] NOT NULL,
	[type] [nvarchar](25) NOT NULL,
 CONSTRAINT [PK_audiences] PRIMARY KEY CLUSTERED 
(
	[room] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UC_room] UNIQUE NONCLUSTERED 
(
	[room] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[audience_msg]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create view [dbo].[audience_msg]
as
select *, concat(room, ' (', type, ')') as [full_audience] from audiences 
GO
/****** Object:  View [dbo].[audience_loading]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE view [dbo].[audience_loading]
as
select 

-- О преподавателе

timetable_id as [timetableId],

--Дисциплина
(select distinct(disciplines.discipline_name) from disciplines where disciplines.discipline_id = timetable.discipline_id) as [discipline_name],
--Аудитория
(select distinct(audience_msg.full_audience) from audience_msg where timetable.room = audience_msg.room) as [room],
--Пара
(select distinct(lesson_msg.full_lesson) where lesson_msg.lesson_number = timetable.lesson_number) as [lesson_number],

-- Дата проведения
timetable.date as [date],

-- Чётность недели
case timetable.evenweek when 1 then 'Четная'else 'Нечетная'end as [evenweek],

--Преподаватель
teachers.name as [teacher_name],

--Кафедра 
(select distinct(department_name) from departments where departments.department_id = teachers.department_id) as [department_name],
-- Должность
(select distinct(position_name) from positions where positions.position_id = teachers.position_id) as [position_name],
-- Звание
(select distinct(rank_name) from ranks where ranks.rank_id = teachers.rank_id) as [rank_name],

-- О группе

timetable.group_id as [group_id],

-- Состав группы
(select distinct(groups.count_people) from groups where groups.group_id = timetable.group_id) as [group_count_people],
-- Специальность группы
(select distinct(specialties.specialty_name) from specialties where groups.id_specialty = specialties.specialty_id) as [specialty_name],
-- Факультет
(select distinct(faculties.faculty_name) from faculties where faculties.faculty_id = groups.id_faculty) as [faculty_name],
-- Форма оубчения
(select distinct(train_types.name) from train_types where groups.id_trainform = train_types.traintype_id) as [train_type_name]

from timetable 
join teachers on timetable.teacher_id = teachers.teacher_id
join disciplines on timetable.discipline_id = disciplines.discipline_id
join lesson_msg on lesson_msg.lesson_number = timetable.lesson_number
join audience_msg on audience_msg.room = timetable.room
join groups on groups.group_id = timetable.group_id
GO
/****** Object:  View [dbo].[teachers_hours]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO













CREATE view [dbo].[teachers_hours]
as

select 

--Имя препода
distinct(name) as [teacherName], 

-- Кафедра преподавателя

(select distinct(department_name) from departments where departments.department_id = teachers.department_id) as [teacher_departmentName],

-- Должность

(select distinct(position_name) from positions where positions.position_id = teachers.position_id) as [teacher_positionName],

-- Звание

(select distinct(rank_name) from ranks where ranks.rank_id = teachers.rank_id) as [teacher_rankName],


-- Кол-во часов за всё время
(select round(count(timetable_id) * 1.5, 1) from timetable where timetable.teacher_id = teachers.teacher_id) as [teacher_workhours],

-- Id препода
teachers.teacher_id as [teacherId]

from timetable join teachers on timetable.teacher_id = teachers.teacher_id;
GO
/****** Object:  Table [dbo].[timetable_changes]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[timetable_changes](
	[timetable_id] [int] NOT NULL,
	[changedescription_id] [int] NOT NULL,
	[room] [int] NOT NULL,
	[teacher_id] [int] NOT NULL,
	[discipline_id] [int] NOT NULL,
	[lesson_number] [int] NOT NULL,
	[date] [date] NOT NULL,
	[evenweek] [bit] NOT NULL,
	[group_id] [int] NOT NULL,
 CONSTRAINT [PK_timetable_changes_1] PRIMARY KEY CLUSTERED 
(
	[timetable_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[changedescription]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[changedescription](
	[changedescription_id] [int] IDENTITY(1,1) NOT NULL,
	[description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_changedescription] PRIMARY KEY CLUSTERED 
(
	[changedescription_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[timetable_changesview]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE view [dbo].[timetable_changesview]
as
SELECT tmch.[timetable_id]

	  -- Причина
      , (select distinct(t1.[description]) from changedescription t1 where t1.changedescription_id = tmch.changedescription_id) as [description]
      , (select distinct(t2.full_audience) from audience_msg t2 where tmch.room = t2.room) as [room]
      , (select distinct(t3.name) from teachers t3 where tmch.[teacher_id] = t3.teacher_id) as [teacher_name]
	  , tmch.teacher_id as [teacher_id]
      ,(select distinct(disciplines.discipline_name) from disciplines where disciplines.discipline_id = tmch.discipline_id) as [discipline_name]
      ,(select distinct(t5.full_lesson) from lesson_msg t5 where t5.lesson_number = tmch.lesson_number) as [lesson_number]
      ,tmch.[date]
      ,case tmch.evenweek when 1 then 'Четная'else 'Нечетная'end as [evenweek]
	  ,tm.group_id
	  ,tm.completed from [timetable_db].[dbo].[timetable_changes] tmch inner join [timetable] tm on tmch.timetable_id = tm.timetable_id;
GO
/****** Object:  UserDefinedFunction [dbo].[GetUnoccupiedRooms]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[GetUnoccupiedRooms] ( 
	@date date,
	@room int
)
RETURNS TABLE
AS 
RETURN 
	SELECT distinct(dirl.lesson_number) as [lessonNumber], dirl.start_time as [startTime], dirl.end_time as [endTime], tm1.room, tm1.date FROM dir_lessons dirl join timetable tm1 on tm1.date = @date

WHERE tm1.room = @room AND NOT EXISTS ( SELECT tm.lesson_number FROM timetable tm WHERE tm.date = @date AND tm.lesson_number = dirl.lesson_number AND tm.room = @room )
GO
/****** Object:  UserDefinedFunction [dbo].[GetChangesUnoccupiedRooms]    Script Date: 17.06.2021 7:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[GetChangesUnoccupiedRooms] ( 
	@date date,
	@room int
)
RETURNS TABLE
AS 
RETURN 
	SELECT distinct(dirl.lesson_number) as [lessonNumber], dirl.start_time as [startTime], dirl.end_time as [endTime], tm1.room, tm1.date FROM dir_lessons dirl join timetable_changes tm1 on tm1.date = @date

WHERE tm1.room = @room AND NOT EXISTS ( SELECT tm.lesson_number FROM timetable_changes tm WHERE tm.date = @date AND tm.lesson_number = dirl.lesson_number AND tm.room = @room )
GO
ALTER TABLE [dbo].[disciplines]  WITH CHECK ADD  CONSTRAINT [FK_disciplines_departments] FOREIGN KEY([department_id])
REFERENCES [dbo].[departments] ([department_id])
GO
ALTER TABLE [dbo].[disciplines] CHECK CONSTRAINT [FK_disciplines_departments]
GO
ALTER TABLE [dbo].[groups]  WITH CHECK ADD  CONSTRAINT [FK_groups_faculties] FOREIGN KEY([id_faculty])
REFERENCES [dbo].[faculties] ([faculty_id])
GO
ALTER TABLE [dbo].[groups] CHECK CONSTRAINT [FK_groups_faculties]
GO
ALTER TABLE [dbo].[groups]  WITH CHECK ADD  CONSTRAINT [FK_groups_specialties] FOREIGN KEY([id_specialty])
REFERENCES [dbo].[specialties] ([specialty_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[groups] CHECK CONSTRAINT [FK_groups_specialties]
GO
ALTER TABLE [dbo].[groups]  WITH CHECK ADD  CONSTRAINT [FK_groups_train_types] FOREIGN KEY([id_trainform])
REFERENCES [dbo].[train_types] ([traintype_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[groups] CHECK CONSTRAINT [FK_groups_train_types]
GO
ALTER TABLE [dbo].[specialties]  WITH CHECK ADD  CONSTRAINT [FK_specialties_faculties2] FOREIGN KEY([faculty_id])
REFERENCES [dbo].[faculties] ([faculty_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[specialties] CHECK CONSTRAINT [FK_specialties_faculties2]
GO
ALTER TABLE [dbo].[teachers]  WITH CHECK ADD  CONSTRAINT [FK_teachers_departments] FOREIGN KEY([department_id])
REFERENCES [dbo].[departments] ([department_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[teachers] CHECK CONSTRAINT [FK_teachers_departments]
GO
ALTER TABLE [dbo].[teachers]  WITH CHECK ADD  CONSTRAINT [FK_teachers_positions] FOREIGN KEY([position_id])
REFERENCES [dbo].[positions] ([position_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[teachers] CHECK CONSTRAINT [FK_teachers_positions]
GO
ALTER TABLE [dbo].[teachers]  WITH CHECK ADD  CONSTRAINT [FK_teachers_ranks] FOREIGN KEY([rank_id])
REFERENCES [dbo].[ranks] ([rank_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[teachers] CHECK CONSTRAINT [FK_teachers_ranks]
GO
ALTER TABLE [dbo].[timetable]  WITH CHECK ADD  CONSTRAINT [FK_timetable_audiences] FOREIGN KEY([room])
REFERENCES [dbo].[audiences] ([room])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[timetable] CHECK CONSTRAINT [FK_timetable_audiences]
GO
ALTER TABLE [dbo].[timetable]  WITH CHECK ADD  CONSTRAINT [FK_timetable_dir_lessons] FOREIGN KEY([lesson_number])
REFERENCES [dbo].[dir_lessons] ([lesson_number])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[timetable] CHECK CONSTRAINT [FK_timetable_dir_lessons]
GO
ALTER TABLE [dbo].[timetable]  WITH CHECK ADD  CONSTRAINT [FK_timetable_disciplines] FOREIGN KEY([discipline_id])
REFERENCES [dbo].[disciplines] ([discipline_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[timetable] CHECK CONSTRAINT [FK_timetable_disciplines]
GO
ALTER TABLE [dbo].[timetable]  WITH CHECK ADD  CONSTRAINT [FK_timetable_groups] FOREIGN KEY([group_id])
REFERENCES [dbo].[groups] ([group_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[timetable] CHECK CONSTRAINT [FK_timetable_groups]
GO
ALTER TABLE [dbo].[timetable]  WITH CHECK ADD  CONSTRAINT [FK_timetable_teachers] FOREIGN KEY([teacher_id])
REFERENCES [dbo].[teachers] ([teacher_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[timetable] CHECK CONSTRAINT [FK_timetable_teachers]
GO
ALTER TABLE [dbo].[timetable_changes]  WITH CHECK ADD  CONSTRAINT [FK_timetable_changes_audiences] FOREIGN KEY([room])
REFERENCES [dbo].[audiences] ([room])
GO
ALTER TABLE [dbo].[timetable_changes] CHECK CONSTRAINT [FK_timetable_changes_audiences]
GO
ALTER TABLE [dbo].[timetable_changes]  WITH CHECK ADD  CONSTRAINT [FK_timetable_changes_changedescription] FOREIGN KEY([changedescription_id])
REFERENCES [dbo].[changedescription] ([changedescription_id])
GO
ALTER TABLE [dbo].[timetable_changes] CHECK CONSTRAINT [FK_timetable_changes_changedescription]
GO
ALTER TABLE [dbo].[timetable_changes]  WITH CHECK ADD  CONSTRAINT [FK_timetable_changes_dir_lessons] FOREIGN KEY([lesson_number])
REFERENCES [dbo].[dir_lessons] ([lesson_number])
GO
ALTER TABLE [dbo].[timetable_changes] CHECK CONSTRAINT [FK_timetable_changes_dir_lessons]
GO
ALTER TABLE [dbo].[timetable_changes]  WITH CHECK ADD  CONSTRAINT [FK_timetable_changes_disciplines] FOREIGN KEY([discipline_id])
REFERENCES [dbo].[disciplines] ([discipline_id])
GO
ALTER TABLE [dbo].[timetable_changes] CHECK CONSTRAINT [FK_timetable_changes_disciplines]
GO
ALTER TABLE [dbo].[timetable_changes]  WITH CHECK ADD  CONSTRAINT [FK_timetable_changes_groups] FOREIGN KEY([group_id])
REFERENCES [dbo].[groups] ([group_id])
GO
ALTER TABLE [dbo].[timetable_changes] CHECK CONSTRAINT [FK_timetable_changes_groups]
GO
ALTER TABLE [dbo].[timetable_changes]  WITH CHECK ADD  CONSTRAINT [FK_timetable_changes_teachers] FOREIGN KEY([teacher_id])
REFERENCES [dbo].[teachers] ([teacher_id])
GO
ALTER TABLE [dbo].[timetable_changes] CHECK CONSTRAINT [FK_timetable_changes_teachers]
GO
ALTER TABLE [dbo].[timetable_changes]  WITH CHECK ADD  CONSTRAINT [FK_timetable_changes_timetable] FOREIGN KEY([timetable_id])
REFERENCES [dbo].[timetable] ([timetable_id])
GO
ALTER TABLE [dbo].[timetable_changes] CHECK CONSTRAINT [FK_timetable_changes_timetable]
GO
ALTER TABLE [dbo].[audiences]  WITH CHECK ADD  CONSTRAINT [CK_audiences] CHECK  ((NOT [type] like '%[^-A-Za-zА-Яа-я. ]%'))
GO
ALTER TABLE [dbo].[audiences] CHECK CONSTRAINT [CK_audiences]
GO
ALTER TABLE [dbo].[departments]  WITH CHECK ADD  CONSTRAINT [CK_departments] CHECK  ((NOT [department_name] like '%[^-A-Za-zА-Яа-я. ]%'))
GO
ALTER TABLE [dbo].[departments] CHECK CONSTRAINT [CK_departments]
GO
ALTER TABLE [dbo].[disciplines]  WITH CHECK ADD  CONSTRAINT [CK_disciplines] CHECK  ((NOT [discipline_name] like '%[^-A-Za-zА-Яа-я. ]%'))
GO
ALTER TABLE [dbo].[disciplines] CHECK CONSTRAINT [CK_disciplines]
GO
ALTER TABLE [dbo].[faculties]  WITH CHECK ADD  CONSTRAINT [CK_faculties] CHECK  ((NOT [faculty_name] like '%[^-A-Za-zА-Яа-я. ]%'))
GO
ALTER TABLE [dbo].[faculties] CHECK CONSTRAINT [CK_faculties]
GO
ALTER TABLE [dbo].[groups]  WITH CHECK ADD  CONSTRAINT [RelationFacultySpecialty] CHECK  (([id_faculty]=[dbo].[get_facultyid]([id_specialty])))
GO
ALTER TABLE [dbo].[groups] CHECK CONSTRAINT [RelationFacultySpecialty]
GO
ALTER TABLE [dbo].[positions]  WITH CHECK ADD  CONSTRAINT [CK_positions] CHECK  ((NOT [position_name] like '%[^-A-Za-zА-Яа-я. ]%'))
GO
ALTER TABLE [dbo].[positions] CHECK CONSTRAINT [CK_positions]
GO
ALTER TABLE [dbo].[ranks]  WITH CHECK ADD  CONSTRAINT [CK_ranks] CHECK  ((NOT [rank_name] like '%[^-()A-Za-zА-Яа-я. ]%'))
GO
ALTER TABLE [dbo].[ranks] CHECK CONSTRAINT [CK_ranks]
GO
ALTER TABLE [dbo].[specialties]  WITH CHECK ADD  CONSTRAINT [CK_specialties] CHECK  ((NOT [specialty_name] like '%[^-A-Za-zА-Яа-я. ]%'))
GO
ALTER TABLE [dbo].[specialties] CHECK CONSTRAINT [CK_specialties]
GO
ALTER TABLE [dbo].[teachers]  WITH CHECK ADD  CONSTRAINT [CK_teachers] CHECK  ((NOT [name] like '%[^A-Za-zА-Яа-я ]%'))
GO
ALTER TABLE [dbo].[teachers] CHECK CONSTRAINT [CK_teachers]
GO
ALTER TABLE [dbo].[timetable]  WITH CHECK ADD  CONSTRAINT [constraint_disciplineteacher] CHECK  (([dbo].[get_getdepartmentid_fromdiscipline]([discipline_id])=[dbo].[get_getdepartmentid_fromteacher]([teacher_id])))
GO
ALTER TABLE [dbo].[timetable] CHECK CONSTRAINT [constraint_disciplineteacher]
GO
ALTER TABLE [dbo].[train_types]  WITH CHECK ADD  CONSTRAINT [CK_train_types] CHECK  ((NOT [name] like '%[^-A-Za-zА-Яа-я. ]%'))
GO
ALTER TABLE [dbo].[train_types] CHECK CONSTRAINT [CK_train_types]
GO
