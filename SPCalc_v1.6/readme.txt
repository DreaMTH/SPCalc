UPDATE 1.5:
	1: NO MORE SINGLE HARDCODED TABLES! -> Three tables: stds -> Contains
students, works -> Contains works and result -> Contains
estimates(student_id, work_id).
	2: Some keys has been changed(run *.exe with [-?] key)
	3: Recreated DataBase logic: static class replaced by wrapper.
	4: Window with datagridview to work with estimates.
USEFULL INFORMATION:
	Work 'Add'(id=1, Name='Add', type='Add', max='5') creates
automatically. Every student recives 0 balls to this work. This feature need
to make students appear in pivot table even they has not recieved any ball for
labs.
To add students from example file use: |application name|.exe -ams st.txt
To add works from example file use: |application name|.exe -amw workss.txt
Please, check *.txt files with examples of usage... 
example.txt -> example.bat
