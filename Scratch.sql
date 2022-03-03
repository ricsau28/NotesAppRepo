UPDATE notes SET notebook_name = "Unfiled" WHERE id=26;

SELECT notebook_name, name, notebooks.id FROM notes
 INNER JOIN notebooks ON notebooks.name = notes.notebook_name
 ORDER BY notebooks.id ASC;
 
SELECT * FROM notebooks ORDER BY name ASC;
UPDATE notebooks SET name = 'Template' WHERE id = 12;
  
SELECT notes.* FROM notes INNER JOIN notebooks WHERE notes.notebook_name = notebooks.name;
UPDATE notes SET notebook_id = 9 WHERE notes.notebook_id IS NULL;

DELETE FROM notes;
DELETE FROM notebooks;
DELETE FROM error_log;
 