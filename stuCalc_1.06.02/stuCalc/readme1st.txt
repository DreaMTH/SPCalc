UPDATE 1.06.02: 2023.10.3 ddr
	методы для работы с текстовиками убраны за ненадобностью.
	ПОЛНОСТЬЮ переработана инициализация окна.
	Для лучшей совместимости начиная с 1.06.02 данная ветка будет разрабатываться на .Net Framework (а не на .NET Core, как раньше)
	P.S. Версия с последними нововведениями языка C# будет так же получать все функциональные обновления и усовершенствоваться параллельно с этой.
	
UPDATE 1.05.02:   2023.09.22  agp

        уточненное описание БД в 
        db.Labs.1.00.08.pdf   лаб.3.2.6-3.2.8
        https://www.researchgate.net/publication/344750300
        добавил бат для  тестирования test.cmd

UPDATE 1.05.01:   2023.09.13  agp

        1) скомпилировал под версию 7
        2) добавить батники для тестирования
        3) Split переделать на массив  и скорее всего с пробелом
                 = lines[0].Split(new char[]{'\t',' '}, StringSplitOptions.RemoveEmptyEntries).Length;

        4) в ./lab10  положил примеры файлов, кроме отчета для  текущей работы
        5) добавил поле includeInFReport -- попадает ли работа в результирущий 
                отчет  для модулей и экзаменов





UPDATE 1.5:
	1: NO MORE SINGLE HARDCODED TABLES! -> Three tables: stds -> Contains
students, works -> Contains works and result -> Contains
estimates(student_id, work_id).
	2: Some keys has been changed(run *.exe with [-?] key)
	3: Recreated DataBase logic: static class replaced by wrapper.
USEFULL INFORMATION:
	Work 'Add'(id=1, Name='Add', type='Add', max='5') creates
automatically. Every student recives 0 balls to this work. This feature need
to make students appear in pivot table even they has not recieved any ball for
labs.
To add students from example file use: |application name|.exe -ams st.txt
To add works from example file use: |application name|.exe -amw workss.txt
Please, check *.txt files with examples of usage... 
