grammar Crit;


program: line* EOF;

line: statement | ifBlock | whileBlock;


statement: (assignment|functionCall) ';';

ifBlock: 'if' expression block ('else' elseIfBlock);

elseIfBlock: block | ifBlock;

whileBlock: WHILE expression block ('else' elseIfBlock);

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

constant: INTEGER | FLOAT | STRING | BOOL | array | NULL;
//arrayIndex: constant '[' INTEGER ']';
INTEGER: [0-9]+;
FLOAT: [0-9]+ '.' [0-9]+;
STRING: ('"' ~'"'* '"') | ('\'' ~'\''* '\'');
BOOL: 'true' | 'false';
array: '[' (constant (',' constant)*)? ']'; 
//index: '[' INTEGER ']';
NULL: 'null';

INDEX: '[' IDENTIFIER ']' | '[' INTEGER ']';
INCREMENTOP: '++' | '--';

block: '{' line* '}';

WS:  [ \t\r\n]+ -> skip;
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]* (INDEX)* ;

Comment
  :  '#' ~( '\r' | '\n' )*
  ;


