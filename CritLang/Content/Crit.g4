grammar Crit;


program: line* EOF;

line: statement | ifBlock | whileBlock;


statement: (assignment|functionCall) ';';

ifBlock: 'if' expression block ('else' elseIfBlock)?;

elseIfBlock: block | ifBlock;

whileBlock: WHILE expression block ('else' elseIfBlock)?;

WHILE: 'while' | 'until';

assignment: IDENTIFIER assignmentOp expression;

assignmentOp: '=' | '*=' | '/=' | '%=' | '+=' | '-=';

functionCall: IDENTIFIER '(' (expression (',' expression)*)? ')';

expression
	: constant							#constantExpression
	| IDENTIFIER						#identifierExpression
	| functionCall						#functionCallExpression
	| '(' expression ')'				#parenthesizedExpression
	| '!' expression					#notExpression
	| expression multOp expression		#multiplicativeExpression
	| expression addOp expression		#additiveExpression
	| expression compareOp expression	#comparisonExpression
	| expression boolOp expression	    #booleanExpression
	// | expression INCREMENTOP			#incrementExpression TODO add incrementOP
	;



multOp: '*' | '/' | '%';
addOp: '+' | '-';
compareOp: '==' | '!=' | '<' | '>' | '<=' | '>=';
boolOp: BOOL_OPERATOR;

BOOL_OPERATOR: 'and' | 'or' | 'xor';

constant: INTEGER | FLOAT | STRING | BOOL | array | dictionary | NULL;
//arrayIndex: constant '[' INTEGER ']';
INTEGER: [0-9]+;
//SEPERATOR: '.' | ',';
FLOAT: [0-9]+ '.' [0-9]+;
STRING: ('"' ~'"'* '"') | ('\'' ~'\''* '\'');
BOOL: 'true' | 'false';
array: '[' ((constant | IDENTIFIER) (',' (constant | IDENTIFIER))*?)? ']'; 
//array: '[' (constoridentifier (',' constoridentifier)*?)? ']';
dictionary: '{' (STRING ':' (constant | IDENTIFIER) (',' (constant ':' (constant | IDENTIFIER)))*?)? '}';
//index: '[' INTEGER ']';
constoridentifier: constant | IDENTIFIER;
NULL: 'null';

fragment INDEX: '[' (IDENTIFIER | INTEGER | STRING) ']';
INCREMENTOP: '++' | '--';

block: '{' line* '}';

WS:  [ \t\r\n]+ -> skip;
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]* (INDEX)* ;

LINECOMMENT: '#' ~[\r\n]* -> skip;
MULTICOMMENT: '/*' .*? '*/' -> skip;
