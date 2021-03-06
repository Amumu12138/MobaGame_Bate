UnityFS    5.x.x 5.3.6f1       �   A   [   C  �  DL  =     � CAB-0e78f8b71d02f85367b1a29e0967d859 �  �  DL       �5.3.6f1       1   �g9�_&�����p���� ! �    O �7  �����B � � H ��  (  s1  �10  `  @ �  �  � �   \   �Q  �j  �  ` �` ` ` ` ` H `  `  �  H 	� 
� �  �m_PathName �   򄫶�����+kPCh,   �  _a@� �� _  _ _ "y` _ 0 t 	x s . �$, 
� S�  �-� 
�  
� � �  0 0 P�  ` P` � P� 9� � �   � H C P   P\K P� PP � bt �	�  �  � 0 p� � S� �n0 H ��� �!�"�#��` $` %X&� &HX'X	H (� H )� � *� SL  �� +` �@  AssetBundleS�reloadTab cFileIDk	 �Container9 bInfo p5 TIndex `Size a# � m_MainA �RuntimeCompatibility m_� ��m_Dependencies m_IsStreamedScene� # �  1�  "� ��>f��_=0� 3�1  ���������6 �� socket_http.lua�	x	  2/  
1s/s�	dinglua B /B c.bytes
L � �   , _�3  - 9�"
-- HTTP/1.1 client support for the Lua language.1 ALuaS� � toolkit �Author: Diego Nehab
� ;
� >�Declare module and im� d�x ;�--
local �� = require(" #")! >url E.url" Xltn12$   Hmime    istringa  ! headers�   * 0bash #_G t�8  4")
@ �ttp = {}0 W_M =  �@�Program constantsc ?�connection )�out in seconds
_M.TIMEOUT = 601 puser agt�field sen4  80est4 �USERAGENT"�_VERSION&��ed schemesSpSCHEMESf0 ["U�"] = true }9 �default �Pdocum� �retrievalK 0POR~ /80�@�Reads MIMEogfrom aN�, unfold��where needed
�A2fun�sreceive� (|,� c)
    ��ine, name, value, err! A
  or�  P-- ge�1rstD S "in@ �:� (r !if 0the� �turn nil1 4endY n 0go ��l a blank_ s is fou. Rwhile �~= "" do 	� @eld-�  � $ � �sskip(2,bb.find(� �"^(.-):%s*(.*)"))I �if not (h )� �"malformed reponse� "� $ =� Qlower] )� 2nex�" (� �might be u&ed1 ���} �C anyj � s$ �FH%s")�  � &..V� � � � �save pair���[~]\ 
 = p.. ", " 
nMelse< � � � � i 
 �?�Extra sources�Osink[=Vg t��-chunked��� ��.setmeta�%({VXgetfdG )6 *  ()Z,4 _dirty4  %()�6}, p i__call= 	��� p size, R` exten0 
��9�$Y  � " =@�tonumber(�Dgsub�0;.*�m"), 16u  �J �winvalid� "� �-- was it�3las?h d 1> 0h +
  #--� ,a� _�terminatgMCRLF? 6 2l, partq � )< #ifo 
�� �n 	"  	�  it,�, read trailEinto
�? �F � b aQ"})��?ink���Wself,�))k �send("0\r\n /")�7�ormat("%X5 �Alen(i �xr O 3.. �  � � gD�Low level�O API�
B310__i�E }

�_M.open(host,;�, create)}
 �Bwith��, �TcL
Stry((T %or Xcp)()�?h =�K c� � �finalize*y� r  0new� �ch:clos�P 5set�ebefore�&ngY c(c:set) ()))$ ; g �  �&))��re everythJorke���.��h ��(method, uriX4req7
QEs %s<]E  � W"GET"N X1elf� 	 ccd /))� � to � vcanonicL
. $�  �cf, v i7 �
\ _ 9 (P 0[f]� !f):�
 
 h�
� )h)1� Ubody(�
 �"epI  $ =	 "or8
c.empty	 6 " = ( Ppump. 
M�we don't know}�in advan� #en�!edMEhope<8bes�0mod� 
�
(@"con�	�-length"4< Pkeep-�(	]� Eall(-U(N ,2 #c)H�astatus3� - =��5� id�
"fy>q0.9 res� s,�Rch do� $ a` �p-- this��just a heuristic, b{Rs wha2
0RFC�@omme�-
S  �� "�# \� otherwise proce�a�� D"*l"e 	�?codP5 ,� �%d*%.%d* (%d [w
� g )� ��^ 4
^Y �9ink�Rink = ��ink.null�-X���t��"transfer-enco!"]ohortcu��*" 0S�tGt�+#ty�@ /if� 
O $by S�	M,_)D�c!09�>�	�1rew	� "-e%d"�8m )O� 1	3c 8COHigh9Q�  ad�0uri�t�ut\t�	@is aKRxy, w��1ful.r,� w'.
2 Z .E ;  �_M.PROXY
 � {�JpathmL 7ath�& �'nil'"),? 3ram""t. 	! Tquery   	 Afragt"  $ }� sbuild(u;��'=4%or/V 	<9  ~ aparse( +)
; .� �]3128
�7 � 6 � 5ort� � l� ��!os��n a�0ity^2.-@�4 A	> ["�-���,'  t ! �   "X, TE"& "te ��}�
h� �  /!ca!"in�m1pasATalong�� �$nd�jssword�� #["&$za� 	;b"Basic  (lV.b64(e 1 	k )))�
� �� �R7 ;�<@' -FJu K
{ )��
wverride�# �,i,��� 		j!i)��! �� � �� r�"S=&,
�#"/  +	?"
}?
?�� � vinZn. Z '  . ,� )�pexplici��mponents�?url� {� ![iea <! =�7
 �3 wB )�/
�2 '��
` 8-'"�  uti� � as~� nai�ib~n� �` N �
WJ I &Q� X �	��_ w&	��shouldredirec�,)�o, �' �FfalsRJ 	� W, "%s/'if2 �[ l��Xmatch` �^([%w][%w%+%-%.]*)%:n ? �	�![ ]~  �X �' :) a_(�P= 301o 2 3 7J �
0 "==? YHEAD"� � R n� � O< 5)]
�`j� 5[:20446 a>= 100�  _< 2006 
F --?award d�&IZ�4, t 3 ![[ &]]tm�
 <sul�,�n ( {1 �1say- 0URL�0 to�!�absolute�Isomew 0erv�- E C 6thaAD' �L �� �G& �	�  �
> 	U	�	 �?= (� f0) + 11 ��  

�� p back a�fhint ww/ed�$FY 	e�$��� swe loop�$!we��pwe wantZ- %  �)Fsure�`no wayi A /it� $	O� #3^8�
h�n},8		�)r ��*it=		�h�	^ ,4  ���hS� � n'� zAmply�e"od��Hdone�A� kn�� h,��  � �  gn
 �%@100-� inb&wessages�' �/doW h�X� V  �	ta#po,#j@ hon� re��)�� ca�
�� al4y�
���*
 �Uerror�

n
	�	�����OtJ)ly|U� )�
	�	�a1o & ^�*#�a generic+0	  �\(u, b#*{}D��X��t�Atarg//t� �
I��*b)- �	 � =?"?b),0 3typ�Cappl�p/x-www-3A-url3��� h
 "POST"J+7_M.�( =��
����	_�	(��b.conca .�)�
4_M.$�@prot��!P ��y3ype. ��0
U$� D f*��3)

  p_M
    