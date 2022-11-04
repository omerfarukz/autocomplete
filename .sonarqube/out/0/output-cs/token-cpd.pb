Ú
L/Users/omr/RiderProjects/autocomplete/AutoComplete/Builders/IIndexBuilder.cs
	namespace 	
AutoComplete
 
. 
Builders 
{ 
public 

	interface 
IIndexBuilder "
{ 
IndexBuilder 
Add 
( 
string 
keyword  '
)' (
;( )
int 
Build 
( 
) 
; 
}		 
}

 ´|
K/Users/omr/RiderProjects/autocomplete/AutoComplete/Builders/IndexBuilder.cs
	namespace		 	
AutoComplete		
 
.		 
Builders		 
{

 
public 

class 
IndexBuilder 
: 
IIndexBuilder  -
{ 
private 
static 
readonly 
byte  $
[$ %
]% &
NewLine' .
=/ 0
Encoding1 9
.9 :
UTF8: >
.> ?
GetBytes? G
(G H
EnvironmentH S
.S T
NewLineT [
)[ \
;\ ]
private 
readonly 
Stream 
_headerStream  -
;- .
private 
readonly 
Stream 
_indexStream  ,
;, -
private 
readonly 

Dictionary #
<# $
string$ *
,* +
uint, 0
>0 1
_keywordDictionary2 D
;D E
private 
readonly 
Stream 
_tailStream  +
;+ ,
private 
readonly 
Trie 
_trie #
;# $
private 
TrieIndexHeader 
_header  '
;' (
private 
HashSet 
< 
string 
> 
	_keywords  )
;) *
public 
IndexBuilder 
( 
Stream "
headerStream# /
,/ 0
Stream1 7
indexStream8 C
,C D
StreamE K

tailStreamL V
=W X
nullY ]
)] ^
{ 	
_headerStream 
= 
headerStream (
;( )
_indexStream 
= 
indexStream &
;& '
_tailStream 
= 

tailStream $
;$ %
_header 
= 
new 
TrieIndexHeader )
() *
)* +
;+ ,
_trie 
= 
new 
Trie 
( 
) 
; 
	_keywords 
= 
new 
HashSet #
<# $
string$ *
>* +
(+ ,
), -
;- .
_keywordDictionary 
=  
new! $

Dictionary% /
</ 0
string0 6
,6 7
uint8 <
>< =
(= >
)> ?
;? @
}   	
public"" 
IndexBuilder"" 
Add"" 
(""  
string""  &
keyword""' .
)"". /
{## 	
_trie$$ 
.$$ 
Add$$ 
($$ 
keyword$$ 
)$$ 
;$$ 
if%% 
(%% 
keyword%% 
!=%% 
null%% 
&&%%  "
!%%# $
	_keywords%%$ -
.%%- .
Contains%%. 6
(%%6 7
keyword%%7 >
)%%> ?
)%%? @
	_keywords&& 
.&& 
Add&& 
(&& 
keyword&& %
)&&% &
;&&& '
return(( 
this(( 
;(( 
})) 	
public// 
int// 
Build// 
(// 
)// 
{00 	
PrepareForBuild11 
(11 
)11 
;11 
var33 

serializer33 
=33 
new33  %
TrieIndexHeaderSerializer33! :
(33: ;
)33; <
;33< =

serializer44 
.44 
	Serialize44  
(44  !
_headerStream44! .
,44. /
_header440 7
)447 8
;448 9
var66 
processedNodeCount66 "
=66# $
TrieIndexSerializer66% 8
.668 9
	Serialize669 B
(66B C
_trie66C H
.66H I
Root66I M
,66M N
_header66O V
,66V W
_indexStream66X d
)66d e
;66e f
return77 
processedNodeCount77 %
;77% &
}88 	
private:: 
void:: 
PrepareForBuild:: $
(::$ %
)::% &
{;; 	$
ReorderTrieAndLoadHeader<< $
(<<$ %
_trie<<% *
.<<* +
Root<<+ /
)<</ 0
;<<0 1
if>> 
(>> 
_tailStream>> 
!=>> 
null>> #
)>># $
{?? $
CreateTailAndModifyNodes@@ (
(@@( )
_trie@@) .
.@@. /
Root@@/ 3
)@@3 4
;@@4 5
}AA 
}BB 	
privateDD 
voidDD $
ReorderTrieAndLoadHeaderDD -
(DD- .
TrieNodeDD. 6
rootNodeDD7 ?
)DD? @
{EE 	
varFF 
indexerQueueFF 
=FF 
newFF "
QueueFF# (
<FF( )
TrieNodeFF) 1
>FF1 2
(FF2 3
)FF3 4
;FF4 5
indexerQueueGG 
.GG 
EnqueueGG  
(GG  !
rootNodeGG! )
)GG) *
;GG* +
varII 
orderII 
=II 
$numII 
;II 
varJJ 
builderJJ 
=JJ 
newJJ "
TrieIndexHeaderBuilderJJ 4
(JJ4 5
)JJ5 6
;JJ6 7
whileLL 
(LL 
indexerQueueLL 
.LL  
CountLL  %
>LL& '
$numLL( )
)LL) *
{MM 
varNN 
currentNodeNN 
=NN  !
indexerQueueNN" .
.NN. /
DequeueNN/ 6
(NN6 7
)NN7 8
;NN8 9
currentNodeOO 
.OO 
OrderOO !
=OO" #
orderOO$ )
;OO) *
builderPP 
.PP 
AddCharPP 
(PP  
currentNodePP  +
.PP+ ,
	CharacterPP, 5
)PP5 6
;PP6 7
ifTT 
(TT 
currentNodeTT 
.TT  
ParentTT  &
!=TT' )
nullTT* .
&&TT/ 1
currentNodeTT2 =
.TT= >

ChildIndexTT> H
==TTI K
$numTTL M
)TTM N
{UU 
currentNodeVV 
.VV  
ParentVV  &
.VV& '
ChildrenCountVV' 4
=VV5 6
currentNodeVV7 B
.VVB C
OrderVVC H
-VVI J
currentNodeVVK V
.VVV W
ParentVVW ]
.VV] ^
OrderVV^ c
;VVc d
}WW 
ifYY 
(YY 
currentNodeYY 
.YY  
ChildrenYY  (
!=YY) +
nullYY, 0
)YY0 1
{ZZ 
var[[ 

childIndex[[ "
=[[# $
$num[[% &
;[[& '
foreach\\ 
(\\ 
var\\  
	childNode\\! *
in\\+ -
currentNode\\. 9
.\\9 :
Children\\: B
)\\B C
{]] 
	childNode^^ !
.^^! "
Value^^" '
.^^' (

ChildIndex^^( 2
=^^3 4

childIndex^^5 ?
++^^? A
;^^A B
indexerQueue__ $
.__$ %
Enqueue__% ,
(__, -
	childNode__- 6
.__6 7
Value__7 <
)__< =
;__= >
}`` 
}aa 
++cc 
ordercc 
;cc 
}dd 
_headerff 
=ff 
builderff 
.ff 
Buildff #
(ff# $
)ff$ %
;ff% &
}gg 	
privateii 
voidii $
CreateTailAndModifyNodesii -
(ii- .
TrieNodeii. 6
rootii7 ;
)ii; <
{jj 	
SerializeKeywordskk 
(kk 
_tailStreamkk )
)kk) *
;kk* +
varmm 
serializerQueuemm 
=mm  !
newmm" %
Queuemm& +
<mm+ ,
TrieNodemm, 4
>mm4 5
(mm5 6
)mm6 7
;mm7 8
serializerQueuenn 
.nn 
Enqueuenn #
(nn# $
rootnn$ (
)nn( )
;nn) *
whilepp 
(pp 
serializerQueuepp "
.pp" #
Countpp# (
>pp) *
$numpp+ ,
)pp, -
{qq 
varrr 
currentNoderr 
=rr  !
serializerQueuerr" 1
.rr1 2
Dequeuerr2 9
(rr9 :
)rr: ;
;rr; <
varss 
currentNodeAsStringss '
=ss( )
currentNodess* 5
.ss5 6
	GetStringss6 ?
(ss? @
)ss@ A
;ssA B
iftt 
(tt 
currentNodett 
==tt  "
roottt# '
)tt' (
{uu 
currentNodevv 
.vv  
PositionOnTextFilevv  2
=vv3 4
$numvv5 6
;vv6 7
}ww 
elsexx 
{yy $
SetPositionOfCurrentNodezz ,
(zz, -
currentNodezz- 8
,zz8 9
currentNodeAsStringzz: M
)zzM N
;zzN O
}{{ 
if}} 
(}} 
currentNode}} 
.}}  
Children}}  (
==}}) +
null}}, 0
)}}0 1
continue~~ 
;~~ 
foreach
ÄÄ 
(
ÄÄ 
var
ÄÄ 
	childNode
ÄÄ &
in
ÄÄ' )
currentNode
ÄÄ* 5
.
ÄÄ5 6
Children
ÄÄ6 >
)
ÄÄ> ?
{
ÅÅ 
serializerQueue
ÇÇ #
.
ÇÇ# $
Enqueue
ÇÇ$ +
(
ÇÇ+ ,
	childNode
ÇÇ, 5
.
ÇÇ5 6
Value
ÇÇ6 ;
)
ÇÇ; <
;
ÇÇ< =
}
ÉÉ 
}
ÑÑ 
}
ÖÖ 	
private
áá 
void
áá &
SetPositionOfCurrentNode
áá -
(
áá- .
TrieNode
áá. 6
currentNode
áá7 B
,
ááB C
string
ááD J!
currentNodeAsString
ááK ^
)
áá^ _
{
àà 	
if
ää 
(
ää 
currentNode
ää 
.
ää 

IsTerminal
ää &
)
ää& '
{
ãã 
currentNode
åå 
.
åå  
PositionOnTextFile
åå .
??=
åå/ 2 
_keywordDictionary
åå3 E
[
ååE F!
currentNodeAsString
ååF Y
]
ååY Z
;
ååZ [
}
çç 
else
éé 
{
èè 
var
ëë 

nodeResult
ëë 
=
ëë  (
GetNearestTerminalChildren
ëë! ;
(
ëë; <
currentNode
ëë< G
)
ëëG H
;
ëëH I
if
íí 
(
íí 

nodeResult
ìì 
.
ìì 
Status
ìì %
==
ìì& (&
TrieNodeSearchResultType
ìì) A
.
ììA B
FoundEquals
ììB M
||
ììN P

nodeResult
îî 
.
îî 
Status
îî %
==
îî& (&
TrieNodeSearchResultType
îî) A
.
îîA B
FoundStartsWith
îîB Q
)
ïï 
{
ïï 
var
ññ  
positionOnTextFile
ññ *
=
ññ+ , 
_keywordDictionary
ññ- ?
[
ññ? @

nodeResult
ññ@ J
.
ññJ K
Node
ññK O
.
ññO P
	GetString
ññP Y
(
ññY Z
)
ññZ [
]
ññ[ \
;
ññ\ ]

nodeResult
óó 
.
óó 
Node
óó #
.
óó# $ 
PositionOnTextFile
óó$ 6
=
óó7 8 
positionOnTextFile
óó9 K
;
óóK L
currentNode
òò 
.
òò   
PositionOnTextFile
òò  2
=
òò3 4 
positionOnTextFile
òò5 G
;
òòG H
}
ôô 
else
öö 
{
õõ 
currentNode
ùù 
.
ùù   
PositionOnTextFile
ùù  2
=
ùù3 4
$num
ùù5 6
;
ùù6 7
}
ûû 
}
üü 
}
†† 	
private
¢¢ "
TrieNodeSearchResult
¢¢ $(
GetNearestTerminalChildren
¢¢% ?
(
¢¢? @
TrieNode
¢¢@ H
currentNode
¢¢I T
)
¢¢T U
{
££ 	
var
§§ 
serializerQueue
§§ 
=
§§  !
new
§§" %
Queue
§§& +
<
§§+ ,
TrieNode
§§, 4
>
§§4 5
(
§§5 6
)
§§6 7
;
§§7 8
serializerQueue
•• 
.
•• 
Enqueue
•• #
(
••# $
currentNode
••$ /
)
••/ 0
;
••0 1
while
ßß 
(
ßß 
serializerQueue
ßß "
.
ßß" #
Count
ßß# (
>
ßß) *
$num
ßß+ ,
)
ßß, -
{
®® 
currentNode
©© 
=
©© 
serializerQueue
©© -
.
©©- .
Dequeue
©©. 5
(
©©5 6
)
©©6 7
;
©©7 8
if
™™ 
(
™™ 
currentNode
™™ 
.
™™  

IsTerminal
™™  *
)
™™* +
return
´´ "
TrieNodeSearchResult
´´ /
.
´´/ 0
CreateFoundEquals
´´0 A
(
´´A B
currentNode
´´B M
)
´´M N
;
´´N O
if
≠≠ 
(
≠≠ 
currentNode
≠≠ 
.
≠≠  
Children
≠≠  (
==
≠≠) +
null
≠≠, 0
)
≠≠0 1
continue
ÆÆ 
;
ÆÆ 
foreach
∞∞ 
(
∞∞ 
var
∞∞ 
	childNode
∞∞ &
in
∞∞' )
currentNode
∞∞* 5
.
∞∞5 6
Children
∞∞6 >
)
∞∞> ?
{
±± 
serializerQueue
≤≤ #
.
≤≤# $
Enqueue
≤≤$ +
(
≤≤+ ,
	childNode
≤≤, 5
.
≤≤5 6
Value
≤≤6 ;
)
≤≤; <
;
≤≤< =
}
≥≥ 
}
¥¥ 
return
∂∂ "
TrieNodeSearchResult
∂∂ '
.
∂∂' (
CreateNotFound
∂∂( 6
(
∂∂6 7
)
∂∂7 8
;
∂∂8 9
}
∑∑ 	
private
ππ 
void
ππ 
SerializeKeywords
ππ &
(
ππ& '
Stream
ππ' -
stream
ππ. 4
)
ππ4 5
{
∫∫ 	
stream
ªª 
.
ªª 
Position
ªª 
=
ªª 
$num
ªª 
;
ªª  
foreach
ºº 
(
ºº 
var
ºº 
item
ºº 
in
ºº  
	_keywords
ºº! *
.
ºº* +
OrderBy
ºº+ 2
(
ºº2 3
f
ºº3 4
=>
ºº5 7
f
ºº8 9
,
ºº9 :
new
ºº; > 
TrieStringComparer
ºº? Q
(
ººQ R
)
ººR S
)
ººS T
)
ººT U
{
ΩΩ  
_keywordDictionary
ææ "
.
ææ" #
Add
ææ# &
(
ææ& '
item
ææ' +
,
ææ+ ,
(
ææ- .
uint
ææ. 2
)
ææ2 3
stream
ææ4 :
.
ææ: ;
Position
ææ; C
)
ææC D
;
ææD E
var
¿¿ 
buffer
¿¿ 
=
¿¿ 
Encoding
¿¿ %
.
¿¿% &
UTF8
¿¿& *
.
¿¿* +
GetBytes
¿¿+ 3
(
¿¿3 4
item
¿¿4 8
)
¿¿8 9
;
¿¿9 :
stream
¡¡ 
.
¡¡ 
Write
¡¡ 
(
¡¡ 
buffer
¡¡ #
,
¡¡# $
$num
¡¡% &
,
¡¡& '
buffer
¡¡( .
.
¡¡. /
Length
¡¡/ 5
)
¡¡5 6
;
¡¡6 7
stream
¬¬ 
.
¬¬ 
Write
¬¬ 
(
¬¬ 
NewLine
¬¬ $
,
¬¬$ %
$num
¬¬& '
,
¬¬' (
NewLine
¬¬) 0
.
¬¬0 1
Length
¬¬1 7
)
¬¬7 8
;
¬¬8 9
}
√√ 
	_keywords
≈≈ 
.
≈≈ 
Clear
≈≈ 
(
≈≈ 
)
≈≈ 
;
≈≈ 
	_keywords
∆∆ 
=
∆∆ 
null
∆∆ 
;
∆∆ 
}
«« 	
}
»» 
}…… É-
U/Users/omr/RiderProjects/autocomplete/AutoComplete/Builders/TrieIndexHeaderBuilder.cs
	namespace 	
AutoComplete
 
. 
Builders 
{ 
internal 
class "
TrieIndexHeaderBuilder )
{ 
private		 
readonly		 
List		 
<		 
char		 "
>		" #
_characterList		$ 2
;		2 3
public "
TrieIndexHeaderBuilder %
(% &
)& '
{ 	
_characterList 
= 
new  
List! %
<% &
char& *
>* +
(+ ,
), -
;- .
} 	
internal "
TrieIndexHeaderBuilder '
AddChar( /
(/ 0
char0 4
	character5 >
)> ?
{ 	
if 
( 
! 
_characterList 
.  
Contains  (
(( )
	character) 2
)2 3
)3 4
{ 
_characterList 
. 
Add "
(" #
	character# ,
), -
;- .
} 
return 
this 
; 
} 	
internal "
TrieIndexHeaderBuilder '
	AddString( 1
(1 2
string2 8
value9 >
)> ?
{ 	
if 
( 
string 
. 
IsNullOrEmpty $
($ %
value% *
)* +
)+ ,
throw 
new 
ArgumentException +
(+ ,
nameof, 2
(2 3
value3 8
)8 9
)9 :
;: ;
foreach   
(   
var   
t   
in   
value   #
)  # $
{!! 
AddChar"" 
("" 
t"" 
)"" 
;"" 
}## 
return%% 
this%% 
;%% 
}&& 	
internal(( 
TrieIndexHeader((  
Build((! &
(((& '
)((' (
{)) 	
var** 
header** 
=** 
new** 
TrieIndexHeader** ,
(**, -
)**- .
;**. /
header++ 
.++ 
CharacterList++  
=++! "
_characterList++# 1
;++1 2
SortCharacterList-- 
(-- 
)-- 
;--  
CalculateMetrics.. 
(.. 
ref..  
header..! '
)..' (
;..( )
return00 
header00 
;00 
}11 	
private33 "
TrieIndexHeaderBuilder33 &
SortCharacterList33' 8
(338 9
)339 :
{44 	
_characterList55 
.55 
Sort55 
(55  
new55  #!
TrieCharacterComparer55$ 9
(559 :
)55: ;
)55; <
;55< =
return66 
this66 
;66 
}77 	
private99 
void99 
CalculateMetrics99 %
(99% &
ref99& )
TrieIndexHeader99* 9
header99: @
)99@ A
{:: 	
header<< 
.<< 
COUNT_OF_CHARSET<< #
=<<$ %
_characterList<<& 4
.<<4 5
Count<<5 :
;<<: ;
header>> 
.>> #
COUNT_OF_CHILDREN_FLAGS>> *
=>>+ ,
header>>- 3
.>>3 4
COUNT_OF_CHARSET>>4 D
/>>E F
$num>>G H
+>>I J
(>>K L
header>>L R
.>>R S
COUNT_OF_CHARSET>>S c
%>>d e
$num>>f g
==>>h j
$num>>k l
?>>m n
$num>>o p
:>>q r
$num>>s t
)>>t u
;>>u v
header?? 
.?? ,
 COUNT_OF_CHILDREN_FLAGS_IN_BYTES?? 3
=??4 5
header??6 <
.??< =
COUNT_OF_CHARSET??= M
/??N O
$num??P R
+??S T
(??U V
header??V \
.??\ ]
COUNT_OF_CHARSET??] m
%??n o
$num??p r
==??s u
$num??v w
???x y
$num??z {
:??| }
$num??~ 
)	?? Ä
;
??Ä Å
header@@ 
.@@ 6
*COUNT_OF_CHILDREN_FLAGS_BIT_ARRAY_IN_BYTES@@ =
=@@> ?
header@@@ F
.@@F G,
 COUNT_OF_CHILDREN_FLAGS_IN_BYTES@@G g
*@@h i
$num@@j k
;@@k l
headerBB 
.BB $
LENGTH_OF_CHILDREN_FLAGSBB +
=BB, -
headerBB. 4
.BB4 5'
COUNT_OF_CHARACTER_IN_BYTESBB5 P
+BBQ R
headerCC. 4
.CC4 5(
COUNT_TERMINAL_SIZE_IN_BYTESCC5 Q
;CCQ R
headerEE 
.EE %
LENGTH_OF_CHILDREN_OFFSETEE ,
=EE- .
headerEE/ 5
.EE5 6$
LENGTH_OF_CHILDREN_FLAGSEE6 N
+EEO P
headerFF/ 5
.FF5 66
*COUNT_OF_CHILDREN_FLAGS_BIT_ARRAY_IN_BYTESFF6 `
;FF` a
headerHH 
.HH 7
+LENGHT_OF_TEXT_FILE_START_POSITION_IN_BYTESHH >
=HH? @
headerHHA G
.HHG H%
LENGTH_OF_CHILDREN_OFFSETHHH a
+HHb c
headerIIA G
.IIG H6
*COUNT_OF_TEXT_FILE_START_POSITION_IN_BYTESIIH r
;IIr s
headerKK 
.KK 
LENGTH_OF_STRUCTKK #
=KK$ %
headerKK& ,
.KK, -7
+LENGHT_OF_TEXT_FILE_START_POSITION_IN_BYTESKK- X
+KKY Z
headerLL& ,
.LL, --
!COUNT_OF_CHILDREN_OFFSET_IN_BYTESLL- N
;LLN O
}MM 	
}NN 
}OO ¯
d/Users/omr/RiderProjects/autocomplete/AutoComplete/Clients/IndexSearchers/FileSystemIndexSearcher.cs
	namespace 	
AutoComplete
 
. 
Clients 
. 
IndexSearchers -
{ 
public 

class #
FileSystemIndexSearcher (
:) *
IndexSearcher+ 8
{ 
private 
readonly 
string 
_headerFileName  /
;/ 0
private		 
readonly		 
string		 
_indexFileName		  .
;		. /
private

 
readonly

 
string

 
_tailFileName

  -
;

- .
public #
FileSystemIndexSearcher &
(& '
string' -
headerFileName. <
,< =
string> D
indexFileNameE R
,R S
stringT Z
tailFileName[ g
=h i
nullj n
)n o
{ 	
_headerFileName 
= 
headerFileName ,
;, -
_indexFileName 
= 
indexFileName *
;* +
_tailFileName 
= 
tailFileName (
;( )
} 	
	protected 
override 
	IndexData $
InitializeIndexData% 8
(8 9
)9 :
{ 	
var 
	indexData 
= 
new 
	IndexData  )
() *
)* +
;+ ,
	indexData 
. 
Header 
= .
"TrieNodeHelperFileSystemExtensions A
.A B
ReadHeaderFileB P
(P Q
_headerFileNameQ `
)` a
;a b
	indexData 
. 
Index 
= 
	GetStream '
(' (
	indexData( 1
.1 2
Header2 8
.8 9
LENGTH_OF_STRUCT9 I
,I J
FileOptionsK V
.V W
RandomAccessW c
)c d
;d e
if 
( 
_tailFileName 
!=  
null! %
)% &
	indexData 
. 
Tail 
=  
	GetStream! *
(* +
$num+ ,
,, -
FileOptions. 9
.9 :
SequentialScan: H
)H I
;I J
return 
	indexData 
; 
} 	
private 
Stream 
	GetStream  
(  !
int! $

bufferSize% /
,/ 0
FileOptions1 <
fileOptions= H
)H I
{ 	
Stream 
indexStream 
=  
new! $

FileStream% /
(/ 0
_indexFileName   
,   
FileMode!! 
.!! 
Open!! 
,!! 

FileAccess"" 
."" 
Read"" 
,""  
	FileShare## 
.## 
Read## 
,## 

bufferSize$$ 
,$$ 
fileOptions%% 
)&& 
;&& 
return(( 
indexStream(( 
;(( 
})) 	
}** 
}++ •
b/Users/omr/RiderProjects/autocomplete/AutoComplete/Clients/IndexSearchers/InMemoryIndexSearcher.cs
	namespace 	
AutoComplete
 
. 
Clients 
. 
IndexSearchers -
{ 
public 

class !
InMemoryIndexSearcher &
:' (
IndexSearcher) 6
{		 
private

 
const

 
int

 
FirstReadBufferSize

 -
=

. /
$num

0 2
*

3 4
$num

5 9
;

9 :
private 
readonly 
string 
_headerFileName  /
;/ 0
private 
readonly 
string 
_indexFileName  .
;. /
private 
readonly 
string 
_tailFileName  -
;- .
public !
InMemoryIndexSearcher $
($ %
string 
headerFileName !
,! "
string 
indexFileName  
,  !
string 
tailFileName 
=  !
null" &
) 	
{ 	
_headerFileName 
= 
headerFileName ,
;, -
_indexFileName 
= 
indexFileName *
;* +
_tailFileName 
= 
tailFileName (
;( )
} 	
	protected 
override 
	IndexData $
InitializeIndexData% 8
(8 9
)9 :
{ 	
var 
	indexData 
= 
new 
	IndexData  )
() *
)* +
;+ ,
	indexData 
. 
Index 
= 
new !!
ManagedInMemoryStream" 7
(7 8
GetBytesFromFile8 H
(H I
_indexFileNameI W
)W X
)X Y
;Y Z
	indexData 
. 
Header 
= .
"TrieNodeHelperFileSystemExtensions A
.A B
ReadHeaderFileB P
(P Q
_headerFileNameQ `
)` a
;a b
if 
( 
_tailFileName 
!=  
null! %
)% &
	indexData   
.   
Tail   
=    
new  ! $!
ManagedInMemoryStream  % :
(  : ;
GetBytesFromFile  ; K
(  K L
_tailFileName  L Y
)  Y Z
)  Z [
;  [ \
return!! 
	indexData!! 
;!! 
}"" 	
private$$ 
byte$$ 
[$$ 
]$$ 
GetBytesFromFile$$ '
($$' (
string$$( .
path$$/ 3
)$$3 4
{%% 	
using&& 
Stream&& 
stream&& 
=&&  !
new&&" %

FileStream&&& 0
(&&0 1
path'' 
,'' 
FileMode(( 
.(( 
Open(( 
,(( 

FileAccess)) 
.)) 
Read)) 
,))  
	FileShare** 
.** 
Read** 
,** 
FirstReadBufferSize++ #
,++# $
FileOptions,, 
.,, 
RandomAccess,, (
)-- 
;-- 
var.. 
streamBytes.. 
=.. 
new.. !
byte.." &
[..& '
stream..' -
...- .
Length... 4
]..4 5
;..5 6
stream// 
.// 
Read// 
(// 
streamBytes// #
,//# $
$num//% &
,//& '
streamBytes//( 3
.//3 4
Length//4 :
)//: ;
;//; <
return00 
streamBytes00 
;00 
}11 	
}22 
}33 ê

o/Users/omr/RiderProjects/autocomplete/AutoComplete/Clients/IndexSearchers/TrieNodeHelperFileSystemExtensions.cs
	namespace 	
AutoComplete
 
. 
Clients 
. 
IndexSearchers -
{ 
internal 
static 
class .
"TrieNodeHelperFileSystemExtensions <
{ 
public		 
static		 
TrieIndexHeader		 %
ReadHeaderFile		& 4
(		4 5
string		5 ;
path		< @
)		@ A
{

 	
var 

serializer 
= 
new  %
TrieIndexHeaderSerializer! :
(: ;
); <
;< =
using 
Stream 
stream 
=  !
new" %

FileStream& 0
(0 1
path 
, 
FileMode 
. 
Open 
, 

FileAccess 
. 
Read 
,  
	FileShare 
. 
Read 
) 
; 
return 

serializer 
. 
Deserialize )
() *
stream* 0
)0 1
;1 2
} 	
} 
} ˆ*
H/Users/omr/RiderProjects/autocomplete/AutoComplete/DataStructure/Trie.cs
	namespace 	
AutoComplete
 
. 
DataStructure $
{ 
internal 
class 
Trie 
{ 
public 
readonly 
TrieNode  
Root! %
;% &
public 
Trie 
( 
) 
{ 	
Root 
= 
new 
TrieNode 
(  
)  !
;! "
Root 
. 
Children 
= 
new 
SortedDictionary  0
<0 1
char1 5
,5 6
TrieNode7 ?
>? @
(@ A
newA D!
TrieCharacterComparerE Z
(Z [
)[ \
)\ ]
;] ^
} 	
public  
TrieNodeSearchResult #
SearchLastNodeFrom$ 6
(6 7
string7 =
keyword> E
)E F
{ 	
if 
( 
keyword 
== 
null 
)  
throw 
new !
ArgumentNullException /
(/ 0
nameof0 6
(6 7
keyword7 >
)> ?
)? @
;@ A
var 
currentNode 
= 
Root "
;" #
for 
( 
var 
i 
= 
$num 
; 
i 
< 
keyword  '
.' (
Length( .
;. /
i0 1
++1 3
)3 4
{ 
var!! 
	foundNode!! 
=!! 
currentNode!!  +
.!!+ ,
GetNodeFromChildren!!, ?
(!!? @
keyword!!@ G
[!!G H
i!!H I
]!!I J
)!!J K
;!!K L
if## 
(## 
	foundNode## 
!=##  
null##! %
)##% &
{$$ 
currentNode&& 
=&&  !
	foundNode&&" +
;&&+ ,
continue'' 
;'' 
}(( 
return++  
TrieNodeSearchResult++ +
.+++ ,!
CreateFoundStartsWith++, A
(++A B
currentNode++B M
,++M N
i++O P
)++P Q
;++Q R
},, 
return//  
TrieNodeSearchResult// '
.//' (
CreateFoundEquals//( 9
(//9 :
currentNode//: E
)//E F
;//F G
}00 	
public22 
bool22 
Add22 
(22 
string22 
keyword22 &
)22& '
{33 	
if44 
(44 
string44 
.44 
IsNullOrWhiteSpace44 )
(44) *
keyword44* 1
)441 2
)442 3
throw55 
new55 !
ArgumentNullException55 /
(55/ 0
nameof550 6
(556 7
keyword557 >
)55> ?
)55? @
;55@ A
var88 
result88 
=88 
SearchLastNodeFrom88 +
(88+ ,
keyword88, 3
)883 4
;884 5
if:: 
(:: 
result:: 
.:: 
Status:: 
==::  $
TrieNodeSearchResultType::! 9
.::9 :
NotFound::: B
)::B C
return;; 
false;; 
;;; 
if== 
(== 
result== 
.== 
Status== 
====  $
TrieNodeSearchResultType==! 9
.==9 :
FoundStartsWith==: I
)==I J
{>> 
var@@ 
prefix@@ 
=@@ 
keyword@@ $
;@@$ %
ifCC 
(CC 
resultCC 
.CC 
LastKeywordIndexCC +
.CC+ ,
HasValueCC, 4
&&CC5 7
resultDD 
.DD 
LastKeywordIndexDD +
>DD, -
$numDD. /
)DD/ 0
{EE 
prefixGG 
=GG 
keywordGG $
.GG$ %
	SubstringGG% .
(GG. /
resultHH 
.HH 
LastKeywordIndexHH /
.HH/ 0
ValueHH0 5
,HH5 6
keywordII 
.II  
LengthII  &
-II' (
resultII) /
.II/ 0
LastKeywordIndexII0 @
.II@ A
ValueIIA F
)JJ 
;JJ 
}KK 
varMM 
newTrieMM 
=MM 
TrieNodeMM &
.MM& '

CreateFromMM' 1
(MM1 2
prefixMM2 8
)MM8 9
;MM9 :
resultNN 
.NN 
NodeNN 
.NN 
AddNN 
(NN  
newTrieNN  '
)NN' (
;NN( )
returnPP 
truePP 
;PP 
}QQ 
ifSS 
(SS 
resultSS 
.SS 
StatusSS 
==SS  $
TrieNodeSearchResultTypeSS! 9
.SS9 :
FoundEqualsSS: E
&&SSF H
!SSI J
resultSSJ P
.SSP Q
NodeSSQ U
.SSU V

IsTerminalSSV `
)SS` a
{TT 
resultWW 
.WW 
NodeWW 
.WW 

IsTerminalWW &
=WW' (
trueWW) -
;WW- .
}XX 
returnZZ 
falseZZ 
;ZZ 
}[[ 	
}\\ 
}]] Ë
Y/Users/omr/RiderProjects/autocomplete/AutoComplete/DataStructure/TrieCharacterComparer.cs
	namespace 	
AutoComplete
 
. 
DataStructure $
{ 
internal 
class !
TrieCharacterComparer (
:) *
	IComparer+ 4
<4 5
char5 9
>9 :
{ 
public 
int 
Compare 
( 
char 
left  $
,$ %
char& *
right+ 0
)0 1
{ 	
return		 
left		 
.		 
	CompareTo		 !
(		! "
right		" '
)		' (
;		( )
}

 	
} 
} ¡
S/Users/omr/RiderProjects/autocomplete/AutoComplete/DataStructure/TrieIndexHeader.cs
	namespace 	
AutoComplete
 
. 
DataStructure $
{ 
[ 
SuppressMessage 
( 
$str  
,  !
$str" 6
)6 7
]7 8
public 

class 
TrieIndexHeader  
{ 
public		 
TrieIndexHeader		 
(		 
)		  
{

 	
CharacterList 
= 
new 
List  $
<$ %
char% )
>) *
(* +
)+ ,
;, -'
COUNT_OF_CHARACTER_IN_BYTES '
=( )
$num* +
;+ ,(
COUNT_TERMINAL_SIZE_IN_BYTES (
=) *
$num+ ,
;, --
!COUNT_OF_CHILDREN_OFFSET_IN_BYTES -
=. /
$num0 1
;1 26
*COUNT_OF_TEXT_FILE_START_POSITION_IN_BYTES 6
=7 8
$num9 :
;: ;
} 	
public 
List 
< 
char 
> 
CharacterList '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
int '
COUNT_OF_CHARACTER_IN_BYTES .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
int (
COUNT_TERMINAL_SIZE_IN_BYTES /
{0 1
get2 5
;5 6
set7 :
;: ;
}< =
public 
int -
!COUNT_OF_CHILDREN_OFFSET_IN_BYTES 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
public 
int 
COUNT_OF_CHARSET #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
int #
COUNT_OF_CHILDREN_FLAGS *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public!! 
int!! ,
 COUNT_OF_CHILDREN_FLAGS_IN_BYTES!! 3
{!!4 5
get!!6 9
;!!9 :
set!!; >
;!!> ?
}!!@ A
public## 
int## 6
*COUNT_OF_CHILDREN_FLAGS_BIT_ARRAY_IN_BYTES## =
{##> ?
get##@ C
;##C D
set##E H
;##H I
}##J K
public%% 
int%% 6
*COUNT_OF_TEXT_FILE_START_POSITION_IN_BYTES%% =
{%%> ?
get%%@ C
;%%C D
set%%E H
;%%H I
}%%J K
public'' 
int'' 
LENGTH_OF_STRUCT'' #
{''$ %
get''& )
;'') *
set''+ .
;''. /
}''0 1
public)) 
int)) $
LENGTH_OF_CHILDREN_FLAGS)) +
{)), -
get)). 1
;))1 2
set))3 6
;))6 7
}))8 9
public++ 
int++ %
LENGTH_OF_CHILDREN_OFFSET++ ,
{++- .
get++/ 2
;++2 3
set++4 7
;++7 8
}++9 :
public-- 
int-- 7
+LENGHT_OF_TEXT_FILE_START_POSITION_IN_BYTES-- >
{--? @
get--A D
;--D E
set--F I
;--I J
}--K L
}00 
}11 •A
L/Users/omr/RiderProjects/autocomplete/AutoComplete/DataStructure/TrieNode.cs
	namespace 	
AutoComplete
 
. 
DataStructure $
{ 
internal

 
class

 
TrieNode

 
{ 
public 
TrieNode 
( 
char 
	character &
)& '
: 
this 
( 
) 
{ 	
	Character 
= 
	character !
;! "
} 	
public 
TrieNode 
( 
) 
{ 	
} 	
public"" 
int"" 
Order"" 
{"" 
get"" 
;"" 
set""  #
;""# $
}""% &
public(( 
int(( 
ChildrenOffset(( !
{((" #
get(($ '
;((' (
set(() ,
;((, -
}((. /
internal.. 
int.. 
ChildrenCount.. "
{..# $
get..% (
;..( )
set..* -
;..- .
}../ 0
public44 
char44 
	Character44 
{44 
get44  #
;44# $
set44% (
;44( )
}44* +
public:: 
TrieNode:: 
Parent:: 
{::  
get::! $
;::$ %
set::& )
;::) *
}::+ ,
public@@ 
IDictionary@@ 
<@@ 
char@@ 
,@@  
TrieNode@@! )
>@@) *
Children@@+ 3
{@@4 5
get@@6 9
;@@9 :
set@@; >
;@@> ?
}@@@ A
publicFF 
boolFF 

IsTerminalFF 
{FF  
getFF! $
;FF$ %
setFF& )
;FF) *
}FF+ ,
publicLL 
intLL 

ChildIndexLL 
{LL 
getLL  #
;LL# $
setLL% (
;LL( )
}LL* +
publicNN 
uintNN 
?NN 
PositionOnTextFileNN '
{NN( )
getNN* -
;NN- .
setNN/ 2
;NN2 3
}NN4 5
publicPP 
voidPP 
AddPP 
(PP 
TrieNodePP  
childPP! &
)PP& '
{QQ 	
ifRR 
(RR 
childRR 
==RR 
nullRR 
)RR 
throwSS 
newSS 
ArgumentExceptionSS +
(SS+ ,
$strSS, 3
)SS3 4
;SS4 5
ChildrenUU 
??=UU 
newUU 
SortedDictionaryUU -
<UU- .
charUU. 2
,UU2 3
TrieNodeUU4 <
>UU< =
(UU= >
newUU> A!
TrieCharacterComparerUUB W
(UUW X
)UUX Y
)UUY Z
;UUZ [
ifXX 
(XX 
ChildrenXX 
.XX 
ContainsKeyXX $
(XX$ %
childXX% *
.XX* +
	CharacterXX+ 4
)XX4 5
)XX5 6
{YY 
varZZ 

existsNodeZZ 
=ZZ  
ChildrenZZ! )
[ZZ) *
childZZ* /
.ZZ/ 0
	CharacterZZ0 9
]ZZ9 :
;ZZ: ;
if[[ 
([[ 
child[[ 
.[[ 
Children[[ "
!=[[# %
null[[& *
)[[* +
{\\ 
foreach]] 
(]] 
var]]  
item]]! %
in]]& (
child]]) .
.]]. /
Children]]/ 7
)]]7 8
{^^ 

existsNode__ "
.__" #
Add__# &
(__& '
item__' +
.__+ ,
Value__, 1
)__1 2
;__2 3
}`` 
}aa 
}bb 
elsecc 
{dd 
childee 
.ee 
Parentee 
=ee 
thisee #
;ee# $
Childrenff 
.ff 
Addff 
(ff 
childff "
.ff" #
	Characterff# ,
,ff, -
childff. 3
)ff3 4
;ff4 5
}gg 
}hh 	
publicjj 
TrieNodejj 
GetNodeFromChildrenjj +
(jj+ ,
charjj, 0
	characterjj1 :
)jj: ;
{kk 	
ifll 
(ll 
Childrenll 
==ll 
nullll  
||ll! #
!ll$ %
Childrenll% -
.ll- .
ContainsKeyll. 9
(ll9 :
	characterll: C
)llC D
)llD E
returnmm 
nullmm 
;mm 
returnoo 
Childrenoo 
[oo 
	characteroo %
]oo% &
;oo& '
}pp 	
publicvv 
stringvv 
	GetStringvv 
(vv  
)vv  !
{ww 	
varxx 
sbxx 
=xx 
newxx 
StringBuilderxx &
(xx& '
)xx' (
;xx( )
varyy 
currentNodeyy 
=yy 
thisyy "
;yy" #
while{{ 
({{ 
currentNode{{ 
!={{ !
null{{" &
&&{{' )
currentNode{{* 5
.{{5 6
Parent{{6 <
!={{= ?
null{{@ D
){{D E
{|| 
sb}} 
.}} 
Insert}} 
(}} 
$num}} 
,}} 
currentNode}} (
.}}( )
	Character}}) 2
)}}2 3
;}}3 4
currentNode~~ 
=~~ 
currentNode~~ )
.~~) *
Parent~~* 0
;~~0 1
} 
return
ÅÅ 
sb
ÅÅ 
.
ÅÅ 
ToString
ÅÅ 
(
ÅÅ 
)
ÅÅ  
;
ÅÅ  !
}
ÇÇ 	
public
ââ 
static
ââ 
TrieNode
ââ 

CreateFrom
ââ )
(
ââ) *
string
ââ* 0
keyword
ââ1 8
)
ââ8 9
{
ää 	
if
ãã 
(
ãã 
string
ãã 
.
ãã  
IsNullOrWhiteSpace
ãã )
(
ãã) *
keyword
ãã* 1
)
ãã1 2
||
ãã3 5
keyword
ãã6 =
.
ãã= >
Length
ãã> D
==
ããE G
$num
ããH I
)
ããI J
throw
åå 
new
åå 
ArgumentException
åå +
(
åå+ ,
nameof
åå, 2
(
åå2 3
keyword
åå3 :
)
åå: ;
)
åå; <
;
åå< =
var
éé 
returnValue
éé 
=
éé 
new
éé !
TrieNode
éé" *
(
éé* +
keyword
éé+ 2
[
éé2 3
$num
éé3 4
]
éé4 5
)
éé5 6
;
éé6 7
if
èè 
(
èè 
keyword
èè 
.
èè 
Length
èè 
==
èè !
$num
èè" #
)
èè# $
{
êê 
returnValue
ëë 
.
ëë 

IsTerminal
ëë &
=
ëë' (
true
ëë) -
;
ëë- .
return
íí 
returnValue
íí "
;
íí" #
}
ìì 
var
ïï 
currentNode
ïï 
=
ïï 
returnValue
ïï )
;
ïï) *
for
òò 
(
òò 
var
òò 
i
òò 
=
òò 
$num
òò 
;
òò 
i
òò 
<
òò 
keyword
òò  '
.
òò' (
Length
òò( .
;
òò. /
i
òò0 1
++
òò1 3
)
òò3 4
{
ôô 
var
öö 
newNode
öö 
=
öö 
new
öö !
TrieNode
öö" *
(
öö* +
keyword
öö+ 2
[
öö2 3
i
öö3 4
]
öö4 5
)
öö5 6
;
öö6 7
currentNode
õõ 
.
õõ 
Add
õõ 
(
õõ  
newNode
õõ  '
)
õõ' (
;
õõ( )
if
ûû 
(
ûû 
i
ûû 
==
ûû 
keyword
ûû  
.
ûû  !
Length
ûû! '
-
ûû( )
$num
ûû* +
)
ûû+ ,
{
üü 
newNode
†† 
.
†† 

IsTerminal
†† &
=
††' (
true
††) -
;
††- .
}
°° 
currentNode
££ 
=
££ 
newNode
££ %
;
££% &
}
§§ 
return
¶¶ 
returnValue
¶¶ 
;
¶¶ 
}
ßß 	
}
®® 
}©© ˚
X/Users/omr/RiderProjects/autocomplete/AutoComplete/DataStructure/TrieNodeSearchResult.cs
	namespace 	
AutoComplete
 
. 
DataStructure $
{ 
internal 
class  
TrieNodeSearchResult '
{ 
public 
TrieNode 
Node 
{ 
get "
;" #
private$ +
set, /
;/ 0
}1 2
public 
int 
? 
LastKeywordIndex $
{% &
get' *
;* +
private, 3
set4 7
;7 8
}9 :
public		 $
TrieNodeSearchResultType		 '
Status		( .
{		/ 0
get		1 4
;		4 5
private		6 =
set		> A
;		A B
}		C D
public 
static  
TrieNodeSearchResult *!
CreateFoundStartsWith+ @
(@ A
TrieNodeA I
nodeJ N
,N O
intP S
lastKeywordIndexT d
)d e
{ 	
var 
result 
= 
new  
TrieNodeSearchResult 1
(1 2
)2 3
;3 4
result 
. 
Status 
= $
TrieNodeSearchResultType 4
.4 5
FoundStartsWith5 D
;D E
result 
. 
Node 
= 
node 
; 
result 
. 
LastKeywordIndex #
=$ %
lastKeywordIndex& 6
;6 7
return 
result 
; 
} 	
public 
static  
TrieNodeSearchResult *
CreateFoundEquals+ <
(< =
TrieNode= E
nodeF J
)J K
{ 	
var 
result 
= 
new  
TrieNodeSearchResult 1
(1 2
)2 3
;3 4
result 
. 
Status 
= $
TrieNodeSearchResultType 4
.4 5
FoundEquals5 @
;@ A
result 
. 
Node 
= 
node 
; 
return 
result 
; 
} 	
public 
static  
TrieNodeSearchResult *
CreateNotFound+ 9
(9 :
): ;
{ 	
var   
result   
=   
new    
TrieNodeSearchResult   1
(  1 2
)  2 3
;  3 4
result!! 
.!! 
Status!! 
=!! $
TrieNodeSearchResultType!! 4
.!!4 5
NotFound!!5 =
;!!= >
return## 
result## 
;## 
}$$ 	
}%% 
}&& à
\/Users/omr/RiderProjects/autocomplete/AutoComplete/DataStructure/TrieNodeSearchResultType.cs
	namespace 	
AutoComplete
 
. 
DataStructure $
{ 
public 

enum $
TrieNodeSearchResultType (
{ 
FoundEquals 
= 
$num 
, 
FoundStartsWith 
= 
$num 
, 
NotFound 
= 
$num 
} 
}		 Ó!
^/Users/omr/RiderProjects/autocomplete/AutoComplete/DataStructure/TrieNodeStructSearchResult.cs
	namespace 	
AutoComplete
 
. 
DataStructure $
{ 
internal 
class &
TrieNodeStructSearchResult -
{ 
public 
long 
AbsolutePosition $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
int #
LastFoundCharacterIndex *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
long !
LastFoundNodePosition )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public $
TrieNodeSearchResultType '
Status( .
{/ 0
get1 4
;4 5
private6 =
set> A
;A B
}C D
public 
static &
TrieNodeStructSearchResult 0!
CreateFoundStartsWith1 F
(F G
longG K
absolutePositionL \
,\ ]
int #
lastFoundCharacterIndex '
,' (
long) -!
lastFoundNodePosition. C
)C D
{ 	
var 
result 
= 
new &
TrieNodeStructSearchResult 7
(7 8
)8 9
;9 :
result 
. 
Status 
= $
TrieNodeSearchResultType 4
.4 5
FoundStartsWith5 D
;D E
result 
. 
AbsolutePosition #
=$ %
absolutePosition& 6
;6 7
result 
. #
LastFoundCharacterIndex *
=+ ,#
lastFoundCharacterIndex- D
;D E
result 
. !
LastFoundNodePosition (
=) *!
lastFoundNodePosition+ @
;@ A
return 
result 
; 
}   	
public"" 
static"" &
TrieNodeStructSearchResult"" 0
CreateFoundEquals""1 B
(""B C
long""C G!
lastFoundNodePosition""H ]
)""] ^
{## 	
var$$ 
result$$ 
=$$ 
new$$ &
TrieNodeStructSearchResult$$ 7
($$7 8
)$$8 9
;$$9 :
result%% 
.%% 
Status%% 
=%% $
TrieNodeSearchResultType%% 4
.%%4 5
FoundEquals%%5 @
;%%@ A
result&& 
.&& !
LastFoundNodePosition&& (
=&&) *!
lastFoundNodePosition&&+ @
;&&@ A
return(( 
result(( 
;(( 
})) 	
public++ 
static++ &
TrieNodeStructSearchResult++ 0
CreateFoundEquals++1 B
(++B C
long++C G
absolutePosition++H X
,++X Y
long++Z ^!
lastFoundNodePosition++_ t
)++t u
{,, 	
var-- 
result-- 
=-- 
new-- &
TrieNodeStructSearchResult-- 7
(--7 8
)--8 9
;--9 :
result.. 
... 
Status.. 
=.. $
TrieNodeSearchResultType.. 4
...4 5
FoundEquals..5 @
;..@ A
result// 
.// 
AbsolutePosition// #
=//$ %
absolutePosition//& 6
;//6 7
result00 
.00 !
LastFoundNodePosition00 (
=00) *!
lastFoundNodePosition00+ @
;00@ A
return22 
result22 
;22 
}33 	
public55 
static55 &
TrieNodeStructSearchResult55 0
CreateNotFound551 ?
(55? @
)55@ A
{66 	
var77 
result77 
=77 
new77 &
TrieNodeStructSearchResult77 7
(777 8
)778 9
;779 :
result88 
.88 
Status88 
=88 $
TrieNodeSearchResultType88 4
.884 5
NotFound885 =
;88= >
return:: 
result:: 
;:: 
};; 	
}<< 
}== ê
V/Users/omr/RiderProjects/autocomplete/AutoComplete/DataStructure/TrieStringComparer.cs
	namespace 	
AutoComplete
 
. 
DataStructure $
{ 
internal 
class 
TrieStringComparer %
:& '
	IComparer( 1
<1 2
string2 8
>8 9
{ 
public 
int 
Compare 
( 
string !
left" &
,& '
string( .
right/ 4
)4 5
{ 	
return		 
string		 
.		 
CompareOrdinal		 (
(		( )
left		) -
,		- .
right		/ 4
)		4 5
;		5 6
}

 	
} 
} ﬂ
R/Users/omr/RiderProjects/autocomplete/AutoComplete/Domain/ManagedInMemoryStream.cs
	namespace 	
AutoComplete
 
. 
Domain 
{ 
public 

class !
ManagedInMemoryStream &
:' (
MemoryStream) 5
{ 
public !
ManagedInMemoryStream $
($ %
byte% )
[) *
]* +
buffer, 2
)2 3
: 
base 
( 
buffer 
) 
{		 	
}

 	
public 
override 
bool 
CanWrite %
=>& (
false) .
;. /
} 
} ˘
J/Users/omr/RiderProjects/autocomplete/AutoComplete/Domain/SearchOptions.cs
	namespace 	
AutoComplete
 
. 
Domain 
{ 
public 

class 
SearchOptions 
{ 
public 
string 
Term 
{ 
get  
;  !
set" %
;% &
}' (
public 
int 
MaxItemCount 
{  !
get" %
;% &
set' *
;* +
}, -
public		 
bool		 &
SuggestWhenFoundStartsWith		 .
{		/ 0
get		1 4
;		4 5
set		6 9
;		9 :
}		; <
}

 
} ˝
I/Users/omr/RiderProjects/autocomplete/AutoComplete/Domain/SearchResult.cs
	namespace 	
AutoComplete
 
. 
Domain 
{ 
public 

class 
SearchResult 
{ 
public 
string 
[ 
] 
Items 
{ 
get  #
;# $
set% (
;( )
}* +
public		 $
TrieNodeSearchResultType		 '

ResultType		( 2
{		3 4
get		5 8
;		8 9
set		: =
;		= >
}		? @
}

 
} ı
L/Users/omr/RiderProjects/autocomplete/AutoComplete/Helpers/BitArrayHelper.cs
	namespace 	
AutoComplete
 
. 
Helpers 
{ 
public 

static 
class 
BitArrayHelper &
{ 
public 
static 
void 
CopyToInt32Array +
(+ ,
this, 0
BitArray1 9
bitArray: B
,B C
intD G
[G H
]H I
arrayJ O
,O P
intQ T
indexU Z
)Z [
{ 	
if 
( 
bitArray 
== 
null  
)  !
throw 
new !
ArgumentNullException /
(/ 0
nameof0 6
(6 7
bitArray7 ?
)? @
)@ A
;A B
if 
( 
array 
== 
null 
) 
throw 
new !
ArgumentNullException /
(/ 0
nameof0 6
(6 7
array7 <
)< =
)= >
;> ?
var 
location 
= 
$num 
; 
for 
( 
var 
i 
= 
$num 
; 
i 
< 
bitArray  (
.( )
Length) /
;/ 0
i1 2
++2 4
)4 5
{ 
if 
( 
i 
% 
$num 
== 
$num 
&&  "
i# $
!=% '
$num( )
)) *
{ 
index 
++ 
; 
location 
= 
$num  
;  !
} 
if 
( 
bitArray 
. 
Get  
(  !
i! "
)" #
)# $
array   
[   
index   
]    
|=  ! #
$num  $ %
<<  & (
location  ) 1
;  1 2
location"" 
++"" 
;"" 
}## 
}$$ 	
}%% 
}&& ´ü
N/Users/omr/RiderProjects/autocomplete/AutoComplete/Readers/TrieBinaryReader.cs
	namespace 	
AutoComplete
 
. 
Readers 
{		 
internal

 
class

 
TrieBinaryReader

 #
{ 
private 
readonly 
BinaryReader %
_binaryReader& 3
;3 4
private 
readonly 
TrieIndexHeader (
_header) 0
;0 1
private 
readonly *
TrieIndexHeaderCharacterReader 7
_headerReader8 E
;E F
public 
TrieBinaryReader 
(  
BinaryReader  ,
binaryReader- 9
,9 :
TrieIndexHeader; J
headerK Q
)Q R
{ 	
_binaryReader 
= 
binaryReader (
;( )
_header 
= 
header 
; 
_headerReader 
= 
new *
TrieIndexHeaderCharacterReader  >
(> ?
header? E
)E F
;F G
} 	
public 
IEnumerable 
< 
string !
>! " 
GetAutoCompleteNodes# 7
(7 8
long 
position 
, 
string 
prefix 
, 
int 
maxItems 
) 	
{ 	
return (
GetAutoCompleteNodesInternal /
(/ 0
position0 8
,8 9
prefix: @
,@ A
maxItemsB J
,J K
newL O
ListP T
<T U
stringU [
>[ \
(\ ]
)] ^
)^ _
;_ `
} 	
public   
IEnumerable   
<   
string   !
>  ! " 
GetAutoCompleteNodes  # 7
(  7 8
long!! 
position!! 
,!! 
string"" 
prefix"" 
,"" 
int## 
maxItems## 
,## 
Stream$$ 
tail$$ 
)%% 	
{&& 	
return'' 
new'' 
List'' 
<'' 
string'' "
>''" #
(''# $(
GetAutoCompleteNodesWithTail''$ @
(''@ A
position''A I
,''I J
tail''K O
,''O P
prefix''Q W
,''W X
maxItems''Y a
)''a b
)''b c
;''c d
}(( 	
private** 
List** 
<** 
string** 
>** (
GetAutoCompleteNodesInternal** 9
(**9 :
long**: >
position**? G
,**G H
object**I O
prefix**P V
,**V W
int**X [
maxItems**\ d
,**d e
List**f j
<**j k
string**k q
>**q r
results**s z
)**z {
{++ 	
var,, 
	character,, 
=,, 
ReadCharacter,, )
(,,) *
position,,* 2
),,2 3
;,,3 4
var-- 

isTerminal-- 
=-- 
ReadIsTerminal-- +
(--+ ,
position--, 4
)--4 5
;--5 6
var// 
	newPrefix// 
=// 
string// "
.//" #
Concat//# )
(//) *
prefix//* 0
,//0 1
	character//2 ;
)//; <
;//< =
if00 
(00 

isTerminal00 
)00 
results11 
.11 
Add11 
(11 
	newPrefix11 %
)11% &
;11& '
var33 
children33 
=33 (
GetChildrenPositionsFromNode33 7
(337 8
_header338 ?
,33? @
position33A I
)33I J
;33J K
if44 
(44 
children44 
!=44 
null44  
)44  !
{55 
for66 
(66 
var66 
i66 
=66 
$num66 
;66 
i66  !
<66" #
children66$ ,
.66, -
Length66- 3
;663 4
i665 6
++666 8
)668 9
{77 
if88 
(88 
results88 
.88  
Count88  %
<88& '
maxItems88( 0
)880 1
{99 (
GetAutoCompleteNodesInternal:: 4
(::4 5
children::5 =
[::= >
i::> ?
]::? @
,::@ A
	newPrefix::B K
,::K L
maxItems::M U
,::U V
results::W ^
)::^ _
;::_ `
};; 
}<< 
}== 
return?? 
results?? 
;?? 
}@@ 	
privateBB 
IEnumerableBB 
<BB 
stringBB "
>BB" #(
GetAutoCompleteNodesWithTailBB$ @
(BB@ A
longBBA E
positionBBF N
,BBN O
StreamBBP V
tailBBW [
,BB[ \
stringBB] c
prefixBBd j
,BBj k
intBBl o
countBBp u
)BBu v
{CC 	
varDD 
positionOnTextFileDD "
=DD# $"
ReadPositionOnTextFileDD% ;
(DD; <
positionDD< D
)DDD E
;DDE F
constEE 
intEE 

bufferSizeEE  
=EE! "
$numEE# $
;EE$ %
usingFF 
varFF 
streamReaderFF "
=FF# $
newFF% (
StreamReaderFF) 5
(FF5 6
tailFF6 :
,FF: ;
EncodingFF< D
.FFD E
UTF8FFE I
,FFI J
falseFFK P
,FFP Q

bufferSizeFFR \
,FF\ ]
trueFF^ b
)FFb c
;FFc d
streamReaderGG 
.GG 

BaseStreamGG #
.GG# $
SeekGG$ (
(GG( )
positionOnTextFileGG) ;
,GG; <

SeekOriginGG= G
.GGG H
BeginGGH M
)GGM N
;GGN O
forII 
(II 
varII 
iII 
=II 
$numII 
;II 
iII 
<II 
countII  %
;II% &
iII' (
++II( *
)II* +
{JJ 
varKK 
lineKK 
=KK 
streamReaderKK '
.KK' (
ReadLineKK( 0
(KK0 1
)KK1 2
;KK2 3
ifLL 
(LL 
!LL 
lineLL 
!LL 
.LL 

StartsWithLL %
(LL% &
prefixLL& ,
,LL, -
StringComparisonLL. >
.LL> ?
OrdinalLL? F
)LLF G
)LLG H
breakMM 
;MM 
yieldNN 
returnNN 
lineNN !
;NN! "
}OO 
}PP 	
privateRR 
charRR 
ReadCharacterRR "
(RR" #
longRR# '
positionRR( 0
)RR0 1
{SS 	
_binaryReaderTT 
.TT 

BaseStreamTT $
.TT$ %
SeekTT% )
(TT) *
positionTT* 2
,TT2 3

SeekOriginTT4 >
.TT> ?
BeginTT? D
)TTD E
;TTE F
varUU 
bytesUU 
=UU 
_binaryReaderUU %
.UU% &

ReadUInt16UU& 0
(UU0 1
)UU1 2
;UU2 3
returnWW 
_headerReaderWW  
.WW  !
GetCharacterAtIndexWW! 4
(WW4 5
bytesWW5 :
)WW: ;
;WW; <
}XX 	
privateZZ 
boolZZ 
ReadIsTerminalZZ #
(ZZ# $
longZZ$ (
positionZZ) 1
)ZZ1 2
{[[ 	
var\\ 
targetPosition\\ 
=\\  
position\\! )
+\\* +
_header\\, 3
.\\3 4'
COUNT_OF_CHARACTER_IN_BYTES\\4 O
;\\O P
_binaryReader]] 
.]] 

BaseStream]] $
.]]$ %
Seek]]% )
(]]) *
targetPosition]]* 8
,]]8 9

SeekOrigin]]: D
.]]D E
Begin]]E J
)]]J K
;]]K L
return__ 
_binaryReader__  
.__  !
ReadBoolean__! ,
(__, -
)__- .
;__. /
}`` 	
privatebb 
uintbb "
ReadPositionOnTextFilebb +
(bb+ ,
longbb, 0
positionbb1 9
)bb9 :
{cc 	
vardd 
targetPositiondd 
=dd  
positiondd! )
+dd* +
_headerdd, 3
.dd3 47
+LENGHT_OF_TEXT_FILE_START_POSITION_IN_BYTESdd4 _
;dd_ `
_binaryReaderee 
.ee 

BaseStreamee $
.ee$ %
Seekee% )
(ee) *
targetPositionee* 8
,ee8 9

SeekOriginee: D
.eeD E
BegineeE J
)eeJ K
;eeK L
returngg 
_binaryReadergg  
.gg  !

ReadUInt32gg! +
(gg+ ,
)gg, -
;gg- .
}hh 	
privatejj 
booljj 
[jj 
]jj 
ReadChildrenFlagsjj (
(jj( )
longjj) -
positionjj. 6
)jj6 7
{kk 	
varll 
targetPositionll 
=ll  
_headerll! (
.ll( )$
LENGTH_OF_CHILDREN_FLAGSll) A
+llB C
positionllD L
;llL M
_binaryReadermm 
.mm 

BaseStreammm $
.mm$ %
Seekmm% )
(mm) *
targetPositionmm* 8
,mm8 9

SeekOriginmm: D
.mmD E
BeginmmE J
)mmJ K
;mmK L
varoo 
childrenoo 
=oo 
_binaryReaderoo (
.oo( )
	ReadBytesoo) 2
(oo2 3
_headeroo3 :
.oo: ;#
COUNT_OF_CHILDREN_FLAGSoo; R
)ooR S
;ooS T
varpp 
bitArraypp 
=pp 
newpp 
BitArraypp '
(pp' (
childrenpp( 0
)pp0 1
;pp1 2
varqq 
childrenFlagsqq 
=qq 
newqq  #
boolqq$ (
[qq( )
_headerqq) 0
.qq0 1
COUNT_OF_CHARSETqq1 A
]qqA B
;qqB C
forrr 
(rr 
varrr 
irr 
=rr 
$numrr 
;rr 
irr 
<rr 
childrenFlagsrr  -
.rr- .
Lengthrr. 4
;rr4 5
irr6 7
++rr7 9
)rr9 :
{ss 
childrenFlagstt 
[tt 
itt 
]tt  
=tt! "
bitArraytt# +
.tt+ ,
Gettt, /
(tt/ 0
itt0 1
)tt1 2
;tt2 3
}uu 
returnww 
childrenFlagsww  
;ww  !
}xx 	
privatezz 
intzz 
ReadChildrenOffsetzz &
(zz& '
longzz' +
positionzz, 4
)zz4 5
{{{ 	
var|| 
targetPosition|| 
=||  
_header||! (
.||( )%
LENGTH_OF_CHILDREN_OFFSET||) B
+||C D
position||E M
;||M N
_binaryReader}} 
.}} 

BaseStream}} $
.}}$ %
Seek}}% )
(}}) *
targetPosition}}* 8
,}}8 9

SeekOrigin}}: D
.}}D E
Begin}}E J
)}}J K
;}}K L
return 
_binaryReader  
.  !
	ReadInt32! *
(* +
)+ ,
;, -
}
ÄÄ 	
private
àà 
long
àà 
?
àà &
GetChildPositionFromNode
àà .
(
àà. /
long
àà/ 3
position
àà4 <
,
àà< =
char
àà> B
	character
ààC L
)
ààL M
{
ââ 	
var
ää 

childIndex
ää 
=
ää 
_headerReader
ää *
.
ää* +
GetCharacterIndex
ää+ <
(
ää< =
	character
ää= F
)
ääF G
;
ääG H
if
ãã 
(
ãã 

childIndex
ãã 
.
ãã 
HasValue
ãã #
)
ãã# $
{
åå 
var
çç 
targetPosition
çç "
=
çç# $
_header
çç% ,
.
çç, -&
LENGTH_OF_CHILDREN_FLAGS
çç- E
+
ççF G
position
ççH P
;
ççP Q
_binaryReader
éé 
.
éé 

BaseStream
éé (
.
éé( )
Seek
éé) -
(
éé- .
targetPosition
éé. <
,
éé< =

SeekOrigin
éé> H
.
ééH I
Begin
ééI N
)
ééN O
;
ééO P
var
êê 

bytesCount
êê 
=
êê  

childIndex
êê! +
.
êê+ ,
Value
êê, 1
/
êê2 3
$num
êê4 5
+
êê6 7
$num
êê8 9
;
êê9 :
var
ëë 
bitwiseChildren
ëë #
=
ëë$ %
_binaryReader
ëë& 3
.
ëë3 4
	ReadBytes
ëë4 =
(
ëë= >

bytesCount
ëë> H
)
ëëH I
;
ëëI J
var
íí 
bitArray
íí 
=
íí 
new
íí "
BitArray
íí# +
(
íí+ ,
bitwiseChildren
íí, ;
)
íí; <
;
íí< =
if
îî 
(
îî 
bitArray
îî 
.
îî 
Get
îî  
(
îî  !

childIndex
îî! +
.
îî+ ,
Value
îî, 1
)
îî1 2
)
îî2 3
{
ïï 
ushort
ññ 

childOrder
ññ %
=
ññ& '
$num
ññ( )
;
ññ) *
for
óó 
(
óó 
var
óó 
i
óó 
=
óó  
$num
óó! "
;
óó" #
i
óó$ %
<
óó& '

childIndex
óó( 2
.
óó2 3
Value
óó3 8
;
óó8 9
i
óó: ;
++
óó; =
)
óó= >
{
òò 
if
ôô 
(
ôô 
bitArray
ôô $
.
ôô$ %
Get
ôô% (
(
ôô( )
i
ôô) *
)
ôô* +
)
ôô+ ,
++
öö 

childOrder
öö (
;
öö( )
}
õõ 
var
ùù 
childrenOffset
ùù &
=
ùù' ( 
ReadChildrenOffset
ùù) ;
(
ùù; <
position
ùù< D
)
ùùD E
;
ùùE F
var
ûû 
newPosition
ûû #
=
ûû$ %
position
ûû& .
+
ûû/ 0
childrenOffset
ûû1 ?
+
ûû@ A

childOrder
ûûB L
*
ûûM N
_header
ûûO V
.
ûûV W
LENGTH_OF_STRUCT
ûûW g
;
ûûg h
return
üü 
newPosition
üü &
;
üü& '
}
†† 
}
°° 
return
££ 
null
££ 
;
££ 
}
§§ 	
private
¶¶ 
long
¶¶ 
[
¶¶ 
]
¶¶ *
GetChildrenPositionsFromNode
¶¶ 3
(
¶¶3 4
TrieIndexHeader
¶¶4 C
header
¶¶D J
,
¶¶J K
long
¶¶L P
parentPosition
¶¶Q _
)
¶¶_ `
{
ßß 	
var
®® 
childrenOffset
®® 
=
®®   
ReadChildrenOffset
®®! 3
(
®®3 4
parentPosition
®®4 B
)
®®B C
;
®®C D
if
©© 
(
©© 
childrenOffset
©© 
==
©© !
$num
©©" #
)
©©# $
return
™™ 
Array
™™ 
.
™™ 
Empty
™™ "
<
™™" #
long
™™# '
>
™™' (
(
™™( )
)
™™) *
;
™™* +
var
¨¨ 
childrenFlags
¨¨ 
=
¨¨ 
ReadChildrenFlags
¨¨  1
(
¨¨1 2
parentPosition
¨¨2 @
)
¨¨@ A
;
¨¨A B
var
≠≠ 
childrenCount
≠≠ 
=
≠≠ 
GetFlaggedCount
≠≠  /
(
≠≠/ 0
childrenFlags
≠≠0 =
,
≠≠= >
true
≠≠? C
)
≠≠C D
;
≠≠D E
var
ÆÆ 
childrenPositions
ÆÆ !
=
ÆÆ" #
new
ÆÆ$ '
long
ÆÆ( ,
[
ÆÆ, -
childrenCount
ÆÆ- :
]
ÆÆ: ;
;
ÆÆ; <
for
∞∞ 
(
∞∞ 
var
∞∞ 
i
∞∞ 
=
∞∞ 
$num
∞∞ 
;
∞∞ 
i
∞∞ 
<
∞∞ 
childrenCount
∞∞  -
;
∞∞- .
i
∞∞/ 0
++
∞∞0 2
)
∞∞2 3
{
±± 
var
≤≤ 
targetPosition
≤≤ "
=
≤≤# $
parentPosition
≤≤% 3
+
≤≤4 5
childrenOffset
≥≥% 3
+
≥≥4 5
i
¥¥% &
*
¥¥' (
header
¥¥) /
.
¥¥/ 0
LENGTH_OF_STRUCT
¥¥0 @
;
¥¥@ A
childrenPositions
∂∂ !
[
∂∂! "
i
∂∂" #
]
∂∂# $
=
∂∂% &
targetPosition
∂∂' 5
;
∂∂5 6
}
∑∑ 
return
ππ 
childrenPositions
ππ $
;
ππ$ %
}
∫∫ 	
internal
ºº (
TrieNodeStructSearchResult
ºº +
SearchLastNode
ºº, :
(
ºº: ;
long
ºº; ?
parentPosition
ºº@ N
,
ººN O
string
ººP V
keyword
ººW ^
)
ºº^ _
{
ΩΩ 	
var
ææ 
result
ææ 
=
ææ (
TrieNodeStructSearchResult
ææ 3
.
ææ3 4
CreateNotFound
ææ4 B
(
ææB C
)
ææC D
;
ææD E
var
¿¿ 
currentPosition
¿¿ 
=
¿¿  !
parentPosition
¿¿" 0
;
¿¿0 1
for
¬¬ 
(
¬¬ 
var
¬¬ 
i
¬¬ 
=
¬¬ 
$num
¬¬ 
;
¬¬ 
i
¬¬ 
<
¬¬ 
keyword
¬¬  '
.
¬¬' (
Length
¬¬( .
;
¬¬. /
i
¬¬0 1
++
¬¬1 3
)
¬¬3 4
{
√√ 
var
ƒƒ 
childPosition
ƒƒ !
=
ƒƒ" #&
GetChildPositionFromNode
ƒƒ$ <
(
ƒƒ< =
currentPosition
ƒƒ= L
,
ƒƒL M
keyword
ƒƒN U
[
ƒƒU V
i
ƒƒV W
]
ƒƒW X
)
ƒƒX Y
;
ƒƒY Z
if
∆∆ 
(
∆∆ 
childPosition
∆∆ !
!=
∆∆" $
null
∆∆% )
)
∆∆) *
{
«« 
if
»» 
(
»» 
i
»» 
==
»» 
keyword
»» $
.
»»$ %
Length
»»% +
-
»», -
$num
»». /
)
»»/ 0
{
…… 
result
   
=
    (
TrieNodeStructSearchResult
  ! ;
.
  ; <
CreateFoundEquals
  < M
(
  M N
childPosition
  N [
.
  [ \
Value
  \ a
)
  a b
;
  b c
break
ÀÀ 
;
ÀÀ 
}
ÃÃ 
currentPosition
ŒŒ #
=
ŒŒ$ %
childPosition
ŒŒ& 3
.
ŒŒ3 4
Value
ŒŒ4 9
;
ŒŒ9 :
continue
œœ 
;
œœ 
}
–– 
if
““ 
(
““ 
i
““ 
!=
““ 
$num
““ 
)
““ 
{
”” 
result
‘‘ 
=
‘‘ (
TrieNodeStructSearchResult
‘‘ 7
.
‘‘7 8#
CreateFoundStartsWith
‘‘8 M
(
‘‘M N
currentPosition
‘‘N ]
,
‘‘] ^
i
‘‘_ `
,
‘‘` a
currentPosition
‘‘b q
)
‘‘q r
;
‘‘r s
}
’’ 
break
◊◊ 
;
◊◊ 
}
ÿÿ 
return
⁄⁄ 
result
⁄⁄ 
;
⁄⁄ 
}
€€ 	
private
›› 
int
›› 
GetFlaggedCount
›› #
(
››# $
bool
››$ (
[
››( )
]
››) *
flags
››+ 0
,
››0 1
bool
››2 6
flag
››7 ;
)
››; <
{
ﬁﬁ 	
var
ﬂﬂ 
count
ﬂﬂ 
=
ﬂﬂ 
$num
ﬂﬂ 
;
ﬂﬂ 
foreach
‡‡ 
(
‡‡ 
var
‡‡ 
t
‡‡ 
in
‡‡ 
flags
‡‡ #
)
‡‡# $
{
·· 
if
‚‚ 
(
‚‚ 
t
‚‚ 
==
‚‚ 
flag
‚‚ 
)
‚‚ 
{
„„ 
++
‰‰ 
count
‰‰ 
;
‰‰ 
}
ÂÂ 
}
ÊÊ 
return
ËË 
count
ËË 
;
ËË 
}
ÈÈ 	
}
ÍÍ 
}ÎÎ ø
\/Users/omr/RiderProjects/autocomplete/AutoComplete/Readers/TrieIndexHeaderCharacterReader.cs
	namespace 	
AutoComplete
 
. 
Readers 
{ 
public 

class *
TrieIndexHeaderCharacterReader /
{ 
private 
readonly 
TrieIndexHeader (
_header) 0
;0 1
private		 
readonly		 

Dictionary		 #
<		# $
char		$ (
,		( )
ushort		* 0
>		0 1
_characterIndex		2 A
;		A B
public *
TrieIndexHeaderCharacterReader -
(- .
TrieIndexHeader. =
header> D
)D E
{ 	
_header 
= 
header 
; 
_characterIndex 
= 
new !

Dictionary" ,
<, -
char- 1
,1 2
ushort3 9
>9 :
(: ;
); <
;< =
for 
( 
ushort 
i 
= 
$num 
; 
i  
<! "
_header# *
.* +
CharacterList+ 8
.8 9
Count9 >
;> ?
i@ A
++A C
)C D
{ 
if 
( 
_header 
. 
CharacterList )
[) *
i* +
]+ ,
==- /
$char0 4
)4 5
continue 
; 
if 
( 
! 
_characterIndex $
.$ %
ContainsKey% 0
(0 1
_header1 8
.8 9
CharacterList9 F
[F G
iG H
]H I
)I J
)J K
_characterIndex #
.# $
Add$ '
(' (
_header( /
./ 0
CharacterList0 =
[= >
i> ?
]? @
,@ A
iB C
)C D
;D E
} 
} 	
internal 
ushort 
? 
GetCharacterIndex *
(* +
char+ /
	character0 9
)9 :
{ 	
if   
(   
!   
_characterIndex    
.    !
ContainsKey  ! ,
(  , -
	character  - 6
)  6 7
)  7 8
return!! 
null!! 
;!! 
return"" 
_characterIndex"" "
[""" #
	character""# ,
]"", -
;""- .
}## 	
internal%% 
char%% 
GetCharacterAtIndex%% )
(%%) *
ushort%%* 0
index%%1 6
)%%6 7
{&& 	
return'' 
_header'' 
.'' 
CharacterList'' (
[''( )
index'') .
]''. /
;''/ 0
}(( 	
})) 
}** Ä
N/Users/omr/RiderProjects/autocomplete/AutoComplete/Searchers/IIndexSearcher.cs
	namespace 	
AutoComplete
 
. 
	Searchers  
{ 
public 

	interface 
IIndexSearcher #
{ 
SearchResult 
Search 
( 
SearchOptions )
options* 1
)1 2
;2 3
void		 
Init		 
(		 
)		 
;		 
}

 
} Í
I/Users/omr/RiderProjects/autocomplete/AutoComplete/Searchers/IndexData.cs
	namespace 	
AutoComplete
 
. 
	Searchers  
{ 
public 

class 
	IndexData 
{ 
public 
TrieIndexHeader 
Header %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public		 
Stream		 
Index		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
public

 
Stream

 
Tail

 
{

 
get

  
;

  !
set

" %
;

% &
}

' (
} 
} ¡4
M/Users/omr/RiderProjects/autocomplete/AutoComplete/Searchers/IndexSearcher.cs
	namespace 	
AutoComplete
 
. 
	Searchers  
{		 
public

 

abstract

 
class

 
IndexSearcher

 '
:

( )
IIndexSearcher

* 8
{ 
private 
	IndexData 

_indexData $
;$ %
private 
TrieBinaryReader  
_reader! (
;( )
public 
virtual 
SearchResult #
Search$ *
(* +
SearchOptions+ 8
options9 @
)@ A
{ 	
if 
( 
options 
== 
null 
)  
throw 
new 
ArgumentException +
(+ ,
nameof, 2
(2 3
options3 :
): ;
); <
;< =
var 
node 
= 
_reader 
. 
SearchLastNode -
(- .
$num. /
,/ 0
options1 8
.8 9
Term9 =
)= >
;> ?
return  
CreateResultFromNode '
(' (
_reader( /
,/ 0
node1 5
,5 6
options7 >
.> ?
Term? C
,C D
optionsE L
)L M
;M N
} 	
public 
void 
Init 
( 
) 
{ 	

_indexData 
= 
InitializeIndexData ,
(, -
)- .
;. /
_reader 
= "
CreateTrieBinaryReader ,
(, -
)- .
;. /
} 	
	protected 
abstract 
	IndexData $
InitializeIndexData% 8
(8 9
)9 :
;: ;
private   
SearchResult    
CreateResultFromNode   1
(  1 2
TrieBinaryReader  2 B
trieBinaryReader  C S
,  S T&
TrieNodeStructSearchResult  U o
node  p t
,  t u
string!! 
keyword!! 
,!! 
SearchOptions!! )
options!!* 1
)!!1 2
{"" 	
var## 
searchResult## 
=## 
new## "
SearchResult### /
(##/ 0
)##0 1
;##1 2
if$$ 
($$ 
node$$ 
.$$ 
Status$$ 
==$$ $
TrieNodeSearchResultType$$ 7
.$$7 8
NotFound$$8 @
)$$@ A
{%% 
searchResult&& 
.&& 

ResultType&& '
=&&( )
node&&* .
.&&. /
Status&&/ 5
;&&5 6
return'' 
searchResult'' #
;''# $
}(( 
string** 
prefix** 
=** 
null**  
;**  !
if++ 
(++ 
node++ 
.++ 
Status++ 
==++ $
TrieNodeSearchResultType++ 7
.++7 8
FoundStartsWith++8 G
)++G H
{,, 
if-- 
(-- 
!-- 
options-- 
.-- &
SuggestWhenFoundStartsWith-- 7
)--7 8
{.. 
searchResult//  
.//  !

ResultType//! +
=//, -
node//. 2
.//2 3
Status//3 9
;//9 :
return00 
searchResult00 '
;00' (
}11 
prefix33 
=33 
keyword33  
.33  !
	Substring33! *
(33* +
$num33+ ,
,33, -
node33. 2
.332 3#
LastFoundCharacterIndex333 J
-33K L
$num33M N
)33N O
;33O P
}44 
else55 
if55 
(55 
node55 
.55 
Status55  
==55! #$
TrieNodeSearchResultType55$ <
.55< =
FoundEquals55= H
)55H I
{66 
prefix77 
=77 

_indexData77 #
.77# $
Tail77$ (
==77) +
null77, 0
?771 2
keyword773 :
.77: ;
	Substring77; D
(77D E
$num77E F
,77F G
keyword77H O
.77O P
Length77P V
-77W X
$num77Y Z
)77Z [
:77\ ]
keyword77^ e
;77e f
}88 
searchResult:: 
.:: 

ResultType:: #
=::$ %
node::& *
.::* +
Status::+ 1
;::1 2
if<< 
(<< 

_indexData<< 
.<< 
Tail<< 
!=<<  "
null<<# '
)<<' (
{== 
searchResult>> 
.>> 
Items>> "
=>># $
trieBinaryReader>>% 5
.>>5 6 
GetAutoCompleteNodes>>6 J
(>>J K
node?? 
.?? !
LastFoundNodePosition?? .
,??. /
prefix@@ 
,@@ 
optionsAA 
.AA 
MaxItemCountAA (
,AA( )

_indexDataBB 
.BB 
TailBB #
)CC 
.CC 
ToArrayCC 
(CC 
)CC 
;CC 
}DD 
elseEE 
{FF 
searchResultGG 
.GG 
ItemsGG "
=GG# $
trieBinaryReaderGG% 5
.GG5 6 
GetAutoCompleteNodesGG6 J
(GGJ K
nodeHH 
.HH !
LastFoundNodePositionHH .
,HH. /
prefixII 
,II 
optionsJJ 
.JJ 
MaxItemCountJJ (
)KK 
.KK 
ToArrayKK 
(KK 
)KK 
;KK 
}LL 
returnOO 
searchResultOO 
;OO  
}PP 	
privateRR 
TrieBinaryReaderRR  "
CreateTrieBinaryReaderRR! 7
(RR7 8
)RR8 9
{SS 	
varTT 
streamTT 
=TT 

_indexDataTT #
.TT# $
IndexTT$ )
;TT) *
varUU 
binaryReaderUU 
=UU 
newUU "
BinaryReaderUU# /
(UU/ 0
streamUU0 6
)UU6 7
;UU7 8
varVV 
trieBinaryReaderVV  
=VV! "
newVV# &
TrieBinaryReaderVV' 7
(VV7 8
binaryReaderVV8 D
,VVD E

_indexDataVVF P
.VVP Q
HeaderVVQ W
)VVW X
;VVX Y
returnXX 
trieBinaryReaderXX #
;XX# $
}YY 	
}ZZ 
}[[ ‹B
[/Users/omr/RiderProjects/autocomplete/AutoComplete/Serializers/TrieIndexHeaderSerializer.cs
	namespace

 	
AutoComplete


 
.

 
Serializers

 "
{ 
internal 
class %
TrieIndexHeaderSerializer ,
{ 
private 
const 
char 
KeyValueSeparator ,
=- .
$char/ 2
;2 3
private 
const 
char 
ItemSeparator (
=) *
$char+ .
;. /
public 
void 
	Serialize 
( 
Stream $
stream% +
,+ ,
TrieIndexHeader- <
header= C
)C D
{ 	
var 

properties 
= 
GetProperties *
(* +
header+ 1
)1 2
;2 3
using 
var 
writer 
= 
new "
StreamWriter# /
(/ 0
stream0 6
,6 7
Encoding8 @
.@ A
UTF8A E
)E F
;F G
foreach 
( 
var 
property !
in" $

properties% /
)/ 0
{ 
writer 
. 
Write 
( 
property %
.% &
Name& *
)* +
;+ ,
writer 
. 
Write 
( 
KeyValueSeparator .
). /
;/ 0
var 
propertyValue !
=" #
property$ ,
., -
GetValue- 5
(5 6
header6 <
)< =
;= >"
SerializePropertyValue &
(& '
propertyValue' 4
,4 5
property6 >
.> ?
PropertyType? K
,K L
writerM S
)S T
;T U
writer 
. 
Write 
( 
Environment (
.( )
NewLine) 0
)0 1
;1 2
} 
}   	
public"" 
TrieIndexHeader"" 
Deserialize"" *
(""* +
Stream""+ 1
stream""2 8
)""8 9
{## 	
var$$ 
header$$ 
=$$ 
new$$ 
TrieIndexHeader$$ ,
($$, -
)$$- .
;$$. /
var%% 

properties%% 
=%% 
GetProperties%% *
(%%* +
header%%+ 1
)%%1 2
;%%2 3
using'' 
var'' 
reader'' 
='' 
new'' "
StreamReader''# /
(''/ 0
stream''0 6
,''6 7
Encoding''8 @
.''@ A
UTF8''A E
)''E F
;''F G
while(( 
((( 
reader(( 
.(( 
Peek(( 
((( 
)((  
>((! "
-((# $
$num(($ %
)((% &
{)) 
var** 
keyValue** 
=** 
reader** %
.**% &
ReadLine**& .
(**. /
)**/ 0
!**0 1
.**1 2
Split**2 7
(**7 8
KeyValueSeparator**8 I
)**I J
;**J K
var++ 
key++ 
=++ 
keyValue++ "
[++" #
$num++# $
]++$ %
;++% &
var,, 
property,, 
=,, 

properties,, )
.,,) *
SingleOrDefault,,* 9
(,,9 :
f,,: ;
=>,,< >
f,,? @
.,,@ A
Name,,A E
==,,F H
key,,I L
),,L M
;,,M N
if-- 
(-- 
property-- 
==-- 
null--  $
)--$ %
throw.. 
new.. "
SerializationException.. 4
(..4 5
$str..5 I
)..I J
;..J K
var00 
propertyValue00 !
=00" #
DeserializeValue00$ 4
(004 5
keyValue005 =
[00= >
$num00> ?
]00? @
,00@ A
property00B J
.00J K
PropertyType00K W
)00W X
;00X Y
property11 
.11 
SetValue11 !
(11! "
header11" (
,11( )
propertyValue11* 7
)117 8
;118 9
}22 
return44 
header44 
;44 
}55 	
private77 
void77 "
SerializePropertyValue77 +
(77+ ,
object77, 2
propertyValue773 @
,77@ A
Type77B F
propertyType77G S
,77S T
StreamWriter77U a
writer77b h
)77h i
{88 	
if99 
(99 
propertyValue99 
!=99  
null99! %
)99% &
{:: 
if;; 
(;; 
typeof;; 
(;; 
List;; 
<;;  
char;;  $
>;;$ %
);;% &
.;;& '
GetTypeInfo;;' 2
(;;2 3
);;3 4
.;;4 5
IsAssignableFrom;;5 E
(;;E F
propertyType;;F R
.;;R S
GetTypeInfo;;S ^
(;;^ _
);;_ `
);;` a
);;a b
{<< 
var== 
list== 
=== 
(==  
List==  $
<==$ %
char==% )
>==) *
)==* +
propertyValue==, 9
;==9 :
var>> 
itemSeparator>> %
=>>& '
$char>>( +
;>>+ ,
foreach@@ 
(@@ 
var@@  
item@@! %
in@@& (
list@@) -
)@@- .
{AA 
writerBB 
.BB 
WriteBB $
(BB$ %
itemSeparatorBB% 2
)BB2 3
;BB3 4
writerCC 
.CC 
WriteCC $
(CC$ %
(CC% &
intCC& )
)CC) *
itemCC+ /
)CC/ 0
;CC0 1
itemSeparatorDD %
=DD& '
ItemSeparatorDD( 5
;DD5 6
}EE 
}FF 
elseGG 
{HH 
writerII 
.II 
WriteII  
(II  !
propertyValueII! .
?II. /
.II/ 0
ToStringII0 8
(II8 9
)II9 :
)II: ;
;II; <
}JJ 
}KK 
}LL 	
privateNN 
objectNN 
DeserializeValueNN '
(NN' (
stringNN( .
valueAsStringNN/ <
,NN< =
TypeNN> B
propertyTypeNNC O
)NNO P
{OO 	
objectPP 
propertyValuePP  
=PP! "
nullPP# '
;PP' (
ifQQ 
(QQ 
typeofQQ 
(QQ 
ListQQ 
<QQ 
charQQ  
>QQ  !
)QQ! "
.QQ" #
GetTypeInfoQQ# .
(QQ. /
)QQ/ 0
.QQ0 1
IsAssignableFromQQ1 A
(QQA B
propertyTypeQQB N
.QQN O
GetTypeInfoQQO Z
(QQZ [
)QQ[ \
)QQ\ ]
)QQ] ^
{RR 
propertyValueSS 
=SS 
valueAsStringSS  -
.SS- .
SplitSS. 3
(SS3 4
ItemSeparatorSS4 A
)SSA B
.TT 
SelectTT 
(TT 
fTT 
=>TT  
ConvertTT! (
.TT( )
ToCharTT) /
(TT/ 0
intTT0 3
.TT3 4
ParseTT4 9
(TT9 :
fTT: ;
)TT; <
)TT< =
)TT= >
.UU 
ToListUU 
(UU 
)UU 
;UU 
}VV 
elseWW 
{XX 
propertyValueYY 
=YY 
ConvertYY  '
.YY' (

ChangeTypeYY( 2
(YY2 3
valueAsStringYY3 @
,YY@ A
propertyTypeYYB N
)YYN O
;YYO P
}ZZ 
return\\ 
propertyValue\\  
;\\  !
}]] 	
private__ 
IEnumerable__ 
<__ 
PropertyInfo__ (
>__( )
GetProperties__* 7
(__7 8
TrieIndexHeader__8 G
header__H N
)__N O
{`` 	
returnaa 
headeraa 
.aa 
GetTypeaa !
(aa! "
)aa" #
.aa# $ 
GetRuntimePropertiesaa$ 8
(aa8 9
)aa9 :
;aa: ;
}bb 	
}cc 
}dd ∏/
U/Users/omr/RiderProjects/autocomplete/AutoComplete/Serializers/TrieIndexSerializer.cs
	namespace		 	
AutoComplete		
 
.		 
Serializers		 "
{

 
internal 
static 
class 
TrieIndexSerializer -
{ 
public 
static 
int 
	Serialize #
(# $
TrieNode$ ,
rootNode- 5
,5 6
TrieIndexHeader7 F
headerG M
,M N
StreamO U
indexV [
)[ \
{ 	
var 
headerReader 
= 
new "*
TrieIndexHeaderCharacterReader# A
(A B
headerB H
)H I
;I J
var 
processedNodeCount "
=# $
$num% &
;& '
var 
serializerQueue 
=  !
new" %
Queue& +
<+ ,
TrieNode, 4
>4 5
(5 6
)6 7
;7 8
serializerQueue 
. 
Enqueue #
(# $
rootNode$ ,
), -
;- .
var 
binaryWriter 
= 
new "
BinaryWriter# /
(/ 0
index0 5
)5 6
;6 7
while 
( 
serializerQueue "
." #
Count# (
>) *
$num+ ,
), -
{ 
var   
currentNode   
=    !
serializerQueue  " 1
.  1 2
Dequeue  2 9
(  9 :
)  : ;
;  ; <
if!! 
(!! 
currentNode!! 
==!!  "
null!!# '
)!!' (
throw"" 
new""  
InvalidDataException"" 2
(""2 3
$str""3 J
)""J K
;""K L
var%% 
characterIndex%% "
=%%# $
headerReader%%% 1
.%%1 2
GetCharacterIndex%%2 C
(%%C D
currentNode%%D O
.%%O P
	Character%%P Y
)%%Y Z
;%%Z [
binaryWriter'' 
.'' 
Write'' "
(''" #
characterIndex''# 1
??''2 4
Convert''5 <
.''< =
ToUInt16''= E
(''E F
$num''F G
)''G H
)''H I
;''I J
binaryWriter(( 
.(( 
Write(( "
(((" #
currentNode((# .
.((. /

IsTerminal((/ 9
)((9 :
;((: ;
SerializeChildren** !
(**! "
binaryWriter**" .
,**. /
header**0 6
,**6 7
headerReader**8 D
,**D E
currentNode**F Q
)**Q R
;**R S
if,, 
(,, 
currentNode,, 
.,,  
Children,,  (
!=,,) +
null,,, 0
),,0 1
{-- 
foreach.. 
(.. 
var..  
	childNode..! *
in..+ -
currentNode... 9
...9 :
Children..: B
)..B C
{// 
serializerQueue00 '
.00' (
Enqueue00( /
(00/ 0
	childNode000 9
.009 :
Value00: ?
)00? @
;00@ A
}11 
}22 
++44 
processedNodeCount44 $
;44$ %
}55 
return77 
processedNodeCount77 %
;77% &
}88 	
private@@ 
static@@ 
void@@ 
SerializeChildren@@ -
(@@- .
BinaryWriterAA 
binaryWriterAA %
,AA% &
TrieIndexHeaderBB 
headerBB "
,BB" #*
TrieIndexHeaderCharacterReaderCC *
headerReaderCC+ 7
,CC7 8
TrieNodeDD 
currentNodeDD  
)DD  !
{EE 	
varGG 
childrenGG 
=GG 
newGG 
BitArrayGG '
(GG' (
headerGG( .
.GG. /
COUNT_OF_CHARSETGG/ ?
)GG? @
;GG@ A
ifHH 
(HH 
currentNodeHH 
.HH 
ChildrenHH $
!=HH% '
nullHH( ,
)HH, -
{II 
foreachJJ 
(JJ 
varJJ 
itemJJ !
inJJ" $
currentNodeJJ% 0
.JJ0 1
ChildrenJJ1 9
)JJ9 :
{KK 
varLL 
	itemIndexLL !
=LL" #
headerReaderLL$ 0
.LL0 1
GetCharacterIndexLL1 B
(LLB C
itemLLC G
.LLG H
KeyLLH K
)LLK L
;LLL M
childrenMM 
.MM 
SetMM  
(MM  !
	itemIndexMM! *
!MM* +
.MM+ ,
ValueMM, 1
,MM1 2
trueMM3 7
)MM7 8
;MM8 9
}NN 
}OO 
varQQ 
childrenFlagsQQ 
=QQ 
newQQ  #
intQQ$ '
[QQ' (
headerQQ( .
.QQ. /,
 COUNT_OF_CHILDREN_FLAGS_IN_BYTESQQ/ O
]QQO P
;QQP Q
childrenRR 
.RR 
CopyToInt32ArrayRR %
(RR% &
childrenFlagsRR& 3
,RR3 4
$numRR5 6
)RR6 7
;RR7 8
foreachTT 
(TT 
varTT 
flagTT 
inTT  
childrenFlagsTT! .
)TT. /
{UU 
binaryWriterVV 
.VV 
WriteVV "
(VV" #
flagVV# '
)VV' (
;VV( )
}WW 
binaryWriterZZ 
.ZZ 
WriteZZ 
(ZZ 
currentNodeZZ *
.ZZ* +
ChildrenCountZZ+ 8
*ZZ9 :
headerZZ; A
.ZZA B
LENGTH_OF_STRUCTZZB R
)ZZR S
;ZZS T
binaryWriter[[ 
.[[ 
Write[[ 
([[ 
currentNode[[ *
.[[* +
PositionOnTextFile[[+ =
??[[> @
$num[[A B
)[[B C
;[[C D
}\\ 	
}__ 
}`` 