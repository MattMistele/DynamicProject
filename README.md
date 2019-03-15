# DynamicProject
Navid's winter 2019 dynamic programming assignment

The files with the beef are Program.cs, DictionaryParser.cs, and DynamicAlgorithm.cs.

I changed the spec to make the output more readable:
  1) Only saving the TOP 50 ORIGINAL DOCUMENTS instead of all possible original documents
  2) Only saving the TOP 100 POSSIBLE SENTENCES instead of all possible sentences
  3) Only saving the output to 1 FILE, instead of possibly millions of different possible original files
  4) When saving top document i, I am taking sentence i from the list of possible sentences, 
     to make the documents different enough from each other. (Instead of each document having 1 sentence change from the previous)
