UnityFS    5.x.x 5.3.6f1       :�   A   [   C  �  ��  :n     � CAB-847a0880e19672fb0b4dc5c34f0d5057 �  �  ��       �5.3.6f1       1   �g9�_&�����p���� ! �    O �7  �����B � � H ��  (  s1  �10  `  @ �  �  � �   \   �Q  �j  �  ` �` ` ` ` ` H `  `  �  H 	� 
� �  �m_PathName �   򄫶�����+kPCh,   �  _a@� �� _  _ _ "y` _ 0 t 	x s . �$, 
� S�  �-� 
�  
� � �  0 0 P�  ` P` � P� 9� � �   � H C P   P\K P� PP � bt �	�  �  � 0 p� � S� �n0 H ��� �!�"�#��` $` %X&� &HX'X	H (� H )� � *� SL  �� +` �@  AssetBundleS�reloadTab cFileIDk	 �Container9 bInfo p5 TIndex `Size a# � m_MainA �RuntimeCompatibility m_� ��m_Dependencies m_IsStreamedScene�  �� �����>� "L�s"1 �dqP�  �  0�  ���������6 H�dis_x86.lua 2�  - 8�
-- LuaJIT x86/x64 disZ
�mbler module.
* �Copyright (C) 2005-2017 Mike Pall. All # �s reserved> � Released under the MIT license. See_ �Notice in luajit.h
� =�This is a help�   u� !by� �machine code dump�Sending small( �snippets to an external_pand mix; 0theJ �output with our own stuff waL �o fragil3po I hadb Abite� `bulletJ  \  wr �yet another�	� �. Oh well ..� 4The� `format(�very similarr awhat nK �m generates. But it hasK �been developed indx0tlyf@look� !at� !op<t!ds fromQIntel� �AMD manuals.� qsupport_ �struction se�  qu ~Tnsive�reflects� �a current�  A r !orq �CPU implements inI  32xP !64 �mode. Yes, t`�ncludes MMX, SSE 2 3   AbSE4.1,   q4a, AVX 2e � even privileged �hypervisorG �(VMX/SVM)s�`Notes:
 *9�(useless) a16 prefix, 3DNow\ spre-586��s are uno.J �No attempt�roptimizC2has�	made -- it's fast enough`� my needs.�:�--

local type =  �sub, byte,��= string.  "  ) A �match, g 6subC  8 g 3subE �lower, rep<  - 3rep,  ]�= require("bit")� `ohex =# . 
�1Map]41stV �  S'32�sUgly? W� read on.� �p_opc1_32 = {
--0x
[0]="addBmr",	 V	 3Brm 	 3Bai 	 ppush es
 !op	 B
"orI 3orV G 3orV E 'orD cD `opc2*"@t1x
"adc� c� c� c� c� cN s� s� 2sbbJ DsbbV	 J   	 J   J dJ !ds� 2� n+n+n+n+n+n+�es:seg","daa� u� u� u� u� u� u� cF � T3x
"xsxtxuxvxwxxsK a� 2cmpF DcmpV	 F   	 F   	 dF � �4x
"incVR", %B
"de  $a
--5x
�U 	 +
  #�
--6x
"sz*S 1aw, !",  op  op 5bouA@rplW�4
"fcg	 @o16:$ C16",�  �timulVrm�&Bs Ainsbf0sVSF Cutsb !VS� �7x
"joBj","jn b  z  "be a C
"js  p pM l g l g> 0--8_arith!B�  V 
 � Btest� 
 3chg1chgSR
"mov( DmovV	 &   %  VmeDleaV 3Wgm�m� �9x
"nop*] �aR|pause| �WaR|repne nop /aR
 
 ) �sz*cbw,cwde,cdqe>0cwd @,cqo� �
� farViw","wait",wfwfwfw a l R
--Ax,#ao	 3Boa #oa,"sb SScmpsb %VS�$ai�zCtosb & 1lod   I Sscasb �B� RC 	 +�ORI",	 ,@
--C��hift!Bmu", V RretBw �","vex*3$lesW  U2$ldsf��� �enterBwu�"veJ fS f�"t3 "Bu  �Ciret6D� 1�  7Bmc c�mN 1aad 0sal Qxlatb�Pp*0", 1 2 3 4 5 6 7�  Ex�2opn�  
 "Bj��jcxzBj,je r �"Ba� 2Vau�2Bua	 "Vu�AallVT$mp "jmf  I dI dI dI !da� F� 2ck:X1urpne:rep   1hlt�c�Cb!Bm 1v!VRc> st "clD!ld � c: Bincd9  }
�Irt(#�	o= 255)
�� (overri�]only)
 64�
aetmeta��({
  [0x06]=false, 7 e , 1, 1, )1e f: 2, (2f 3 3: (60 )61 2 13]=��xdVrDmt" 7�
C32:"Q 4rrex*",  1 b 2 x 3 bH 4 2r", 5 b 6 x 7 I 8 w% 9 I a I b I c r� %4d b e x f � 8Y(9a c� �A #c5 2 5(d4> )d5 	Qeh �
}, { __} )}92nd�(0F xx). True CISC.�l. Hey, I told youPA � �D/SSE@�: (none)�B|o16��, -|F3|66|F2hspsldt!DmMasgdt!U #la�1lsl	 �nil,"sys�61ts" V vB"wb	 / 3ud1
 $Betch^0fem5	�"3dnowMrmuo�movupsXrm|&AsXrv
 4upd d %",+ #mr+ 4mvr+  d + !hlW V$movla Cldup lc d �	ld # runpcklp� | �  h (hp� %lh� h� h� / � h� h� RtS�hintnopV 
% 1 jrmovUmx$4Umy
 $xm
 y
 3mz$�  #zm E a� | � a� a� `cvtpi22 �Mm|cvtsi2CVmt| d d S %nt6nts &pd j �tps2piMrXO ttss2siV )pd d � : 	9 	8 	7 bucomis   a@wrms/Qrdtsc  p=	!ys�	 3xit�agetseccpc3*38  a  ETcmovow7ovn b #bV� Gmovz  &be a/ s/  p/ 'po0 l g$ l k��movmskps~3$|| d  +2qrt� 
 s
 %pd �r, r-  �C"rcp $cp �    n n$or %or/ x x  �^ Eadds
 %pd B $mu�Emuls
 %pd U �� �& �� �s5 &dq  B #dq  l� Esubs
 %pd � i"Emins
 %pd � 3div+ Edivs
 %pd U $axU 5axs
 %pd U )p�2bwP@  *wd $dq �acksswbP� tpcmpgtb  w d: u: 3z hz hz $dw@ #||� 3qdq�  h �CPrVS�#qM�$qu$qa �Qpshuf�| 5hwX d l  � �Aw!Pv� d  �q!Mvmu|| 4q!X. Xcmpeq>9eqw  L *|4  vmiUL�extrqXmuu$|in� � uu�!vm�4Urm' (rm& E$",
F"||hVhuSh�h��1SmMjqNTVSmXrk$mrk
 a�}3joV� b  z  "be #aV�  p pM �g l 5gVjUAsetoVEsetn
 b 
 z 
 $be
 "aB�5ets' 
 p' pa l	 g l
 5gBmf	 @cpui�
b{0shlu c�B gB 	 1rsm@ sA rA rA s"fxsave0�1cmp��9 3$ls�CbtrV�	#lf 6$lgvzxVrBmt W�r|popcnt( Qud2DpN �3btcW 3bsfj s�2|lz6 T|bsrWe se se Hx�x�� � u|W &pd   � c	AiVmr�Ppinsr�W� wBwDrP� 6\ | H FB!Qmp� 1wapd
 2�|/'ub<�v"psrlwP d q�&ad�Fmull� �"Mm�32qM�p� bV$ | /
epsubus � Eminu 8and� 5  5 )ax5 n6 Fpavgn 'ra� a�%vg�  � 7huw� h� dd 2| d* 	� 	�  (or� 	� 	�  x3 iR|||ldk l�l�8llq� u�6add�5sad�Dmaskg| l$� � w d 	r� w 1 (ud�P[255]�   
�athree-�_�. Can't �# �eir next inventi�3c`["38"]  ��[66] 0f 38 xx4� h� h� 'ph�
h	 �	 d 9 FsignhHignw d�(rsR	Wermil� �6!1x0 �blendvbXrma   "ps  d  _ n  �H�||broadcas�)", d �f128XrlXm�!paQ/ 4dPr& ��DsxbwW  d q w5 ! � v�
�	 aI 85dwX�X9 d 2$ 0o� z� z� z� z� z� 6zxd Iermd� 6gtq� �b  � *uw d= )ax= � *uw �%4x�3 h�Josuw�K �VrlvVS/ Gsrav> &sl �P[0x58!/�d�  9 q = a �i��N[0x7c bc 7c wB 8B $8c$ ) kS� +8e  �$vr\ A? baesencZd Clast  e (de6 f 6 �S[0xf0" r|crc32T�	  1 1  7 � sarxVrmv| shl
   L ,
�a� a� x0��y�d ��	h+muh 2�	: r� 9  d �
 � �p#  �� w apalignE�I� �xtrbVmXr�   4VmS   Sactps#  D �� 9lXm*  klXmYru��� � >] � # " S3�3S6 i�  R� i� \��%dpu  1 d 2 mU	 4�Dclmu� 6  6�  A�Rvmb",�	 d 7 � 8� �mpestrmXr+61 4iXr� '62 i7 3 7 �keygenassi.u1 ro�u�}
�)�)�0F 01 C0-FF (!b groupK,�register operands�"vm
00xc�"vm?  2 blaunch 3 �resume",�Rmxoff� !c8?Rnitor"  �md" 8D !un !d9 v "dav 2oad b  �A  �Bstgi  �$cl  �bskinit  �sinvlpgaG   ]"gsq& �,p"i&FPc!An�0 th�*tt stack!."+s(,?_#fp�� D8-DF 00-BF:���a memory�./ ~QfaddF�2mul	 5com	 p
 5sub	 r
 5div	  d
"fldF)3fst2 tD cldenvV 1cwW
 5nst 2nst � �A
"fiaddD  i
 6com
 "pDz Cisub  �  6div
 $rD[ B1 lZ Esttp 	 %pD� �ld twordFmp� p U !C
LGLGC	 $pGC	 $rGC	 $rG� DRG� QQGY	 frstorD� n�� TsTETWTWJ
 &pWTWTWJ
 r[ FTW�  	 Y bR8 � bX5 Qc "xx��|pseudo-���f�f�f�f�f�f�f�f� 9�Q 0xch	 0{"ff'}%P{"fchPFfabs�Q"ftst5 Aam"}' 2ld14l2t	 e	 #pi #g2	 n	 z= !2x=%Qfyl2x�$1tan a	 ~ "re, RdecstZn
 K    K p)  � Ainco� r�2 Ccale D6s"}��^Fmove b 2uFf{ pC��^: nF nS nT &
{qAnclen�}h i�
 ��5 to/ � !  +  %  �1fre� f	 u�u�	E� p� pBA� r�p�  p� F%
{86 ax4p5z Zbfp[126[+ 
a#Di3@bkeyV3%sp�3a ModRM/#S �!  �+� @", "  c 1sbb n !su x# `cmp" } $zE 0rol r# "rc c 2shl  a #arF v'F 
,9i", 1not  ;- �  i 1div   P vP �)	 P  �'O !in6dec d !, �($4 "$�+YP
	   �( �(D 
.m^#ld] 
 4tBl l  ( w; g; bvm*$sg0  i '$l l � 0sms�& �2 "l % �	a b_ &  !"bX 2bts r 3�?  2*, T8bQmp A16bX\ � (  m �vmptrld|�
�n|vmclear" #stv R� �" a 9llw? d? d? d? d? q? .q"A&
z < � /q"A  � K �$ 
!fx		Rdmxcs #st &
	v�"lfenceDp$< s tclflush� n%y  &", w* t+ 8nta. )t0 1 42" ��6;�s�1	XnameseCregsaB]Ud b ad  bhj1"r8l"r9 #10 1 2 3 4 #5bF?B64{ "sp  #si�l�� /W x  d "bx ~ } | � wy wy  1 2 3 4 5@Dy ez e{ e| e} e~ e e� e� d� d�  1 2 3 4 5�Q� r� r� r� r� r� r� r� �  ~ } | { z y My !mm4 2mm1 2 3 4 5 6 7y 9 "A }, �: �>�ext!
  X� xR xS xT xU xV xW xX x� Cxmm8 9I R S $12	 U $14	 Y� y� y� y� y� y� y� y� y� y� y�  I R S $12	 U � y� }.4seg1 �/ s d f g Degr6	  �
�Nsize�Gsz2n�!1,�!2,/!4,�B8, M  �116,7+32� "z2�<O "h
"T "�"Y ""d %"q M o #"x  t %"y) FG ( G6 v �<� *s/�q se.DO�?�a nicely�< �>l�@hn� �=��2fun?�putop(ctx, text,2
  ,  E r, pos, b<Bctx.  
  %""/ 4hma% 0hex'A �
f 0> 0 n
� , i=) � start,pos-1 do
o	m Chex.u=r("%02X"�=(� ai, i))2 1end @if #< >s  o L �sub(hex,�  r)..". "> 8lses rrep(" ") -Q 3+2)d j %if-e  A! = u.." "..  2  � 9o161  �9 .   ;" != �0
; :a32;   	;   ;  `>;  � A �>A x�t>C = ( ,@0"w",AT"").. �B&"r R" x" x< b b< 2vexB$"l"  v  #~=��  �Ev".. Dt, /""4 x7 4< �?(> "^3�  T= f:vexS = � B vC � a�%; Y x b K 
& E v8 	=seg`ext2, � %[� [`vseg..":�n0`( ��Z2 � �  �7
, 	� 
�� 3imm� 2immT   � $sy& �symtab[imm]�    `\t->".1 	M�6ut(��8x  %s%s\n"iVaddr+. ��#))A 7mrm� >   =�$
 �   =h
� �G � BzNflag=�"  es�� �;�zx
�8�%PFallb��n�&lei%�F=end. 4pos0top+1
 CYretur�&"(R ?)")� @unkn(H��   � { 8 x R'  an@ediaf�specified!	� bgetimm[ T"n)`�pos+n-1 >!?� R	���
_1�Hb1 =@� !po�� 5b1
�G 2G O, b2K ++1M s+b2*256T F �, b3, b4N 3N �S �+b3*65536+b4*1677721h ���   u��Process pC
#rnuF7�I�ID
2pat� ,K @
8  �Fqgs, sz,`<�, sp, rm, sc, rx, sdispx
 #, �
 2,�0

 �QChars�K�: 1DFGIMPQRSTUVWXYabcdfgijlmoprstuvwxyz
�
p=�G0(pa�4.")�
�x4�p�V� &U"6E	w� sz�=Q";�2 >2
5 -W"'

4 � � +	v 0  %D"Z#  !]][sz]
` T� %w +Bw F *B"B �YMN .�'or \ � ��WDQMXYFG]")j p� !sz� Xf�� .Y"Z� P� ��
X�
M� YS�  ! = .�JN(sz)6 s6 �"11);�%ot�	� x�q <= 127� L	P+0x%0u ! 2)
	A%or! -! A256-% U ��� u� @� 
� � b� @��imm/16+1]� w� 2 2� og �(zoffset]�%
	� 1� 4� 1� ? 2? ,+4A 2A � [�8 ]� 2 F1)
	� 8�� � } y 	r 4r .ndli�I  D8�rP	K �z���� L   4� �n��Q�(�  0  b> 0x7f �#	 n 3(0x @f+1)�)
	bA  O
5_8�-�	� ? > 
= +ndN�n`jT!z�SB��  t1 u-��	* �2147483641 �4294967296 1 ! + +� k Q � � 	
{ D� W� * �4�%�#lo� a% 0x10 zP2x%06[  of-lo) // J, lo� ? R"0x".S(=�Ri r�#-1 =)%8�
b8 @N + 8�f?r+1Z	 a�' % c%  w" d" /dx"  1" 1! UoJ�mr^mrm
	" �? > �"	 > �	zD	� 6 4%8;> ( �-rm)/8
	sp! "sp! !di$ !""� # 8< 3�  e  E �  O  sc� � y �  sc� ; 4(sc�  x  x ��0 )rx�x�8 &rx� # r�"  )>y!5!93dsz~^   ~�! 8 ��6dsz�% !&  � ]*rm� � � x (R -sc�1Ga32)� � a ) � %
	Yca  �B  � P '<=�X +X T � 

; #��
I #  0
		 (�  �%� 	-+08� � +5dsz�	%rm��  /rm�	2r�Yp4 r�
Mm�3� rm�G	�a�P�D   5 sA�"8�� � av ��D T"rip"�Fu x'
� �srm.."+"&rx� x� ec,&  srx�*"..(2^sc)�	� 8 %s�]~
	�a� �`[aRrgp)or Et"))^TYuck.� 	�  �
�x�
�r�&spm	* g* �!1 pQSuppr��./ fst"..rm( x� '��l
!^"CR8"
R0"CR#p5v� �g  ?+1]� \ yD� (  z( T( l( 	c� t|
�error("bad�``"..paJ-'"�
��� = 	"1.",� �x�m�1"�qorward �:,rap@2act' �): acA.?.3!mr
U��
�
r�u� �	i) If �	v
3	'�� QA  *@DispX Ic hagor8R �$on�d9 �o�vpatgrp)  	� -�D F"%|"��RPvaria�b
� Kk|!p"�s"=Tp>�"%|([^%|]*)�"%|    �
-,�G  -�3 ^) �H=(gi/--�  � �--XXX failN&S66 f2�Cqf1 06  >� eax,WORD PTR [esi]@  re�3 ��branches?
 �$�ureg$mem��u�;x�K ; R>= 19�3$]*V$(.*)4"@ �#��%3 Y�� �"^([a-z0-9 ]*)�  Ma ���F  
	[/#_ 1�]E*G�gu3�'5maphO:4map=1cma�0�� - [']
�O + 1H�� |4ag"t; {4x4 ��first ch�"#ftLjw
]0* ��;	�87out�A  ["#A(� )n,sOU +c�$fak �G 2<.�*;�* D Q G
  V U T& M X P& F G Y& D ol$i +".:/Dctx[: <+�32)].< 0-�$?> 5�
 � xLimit #� �PChain�"%alo�"#by? � *�  	�� #"Us�d$ 9]k7� !� 
�:��7� p[((mrm- �%8))/8)%8+1]� 	�o16,o32[,o64]X/sz� ��e��,@� }p6  `
�� /nd�� �^[^,]*")s� @8Two�K0  B\��^Lo RLq 3q ,3]v �Bl /vmj 	g 3vm[�l PFloat�
_point� /fpwUd,(%8�%"dx=R*8 + xw(if=	�4   +�a
f ��;5idx�* m�)� �2	�"atC5o�7REX/�)�s.�	aOnly 1c  Ej  �*Bowed�
�%	� �` [ qt�_~-;ex"�	� v� `�  \.��%��&v-�!	Ibb>�if b < 12�~	�� :48 �3L�A%32;� �(b-m)/32N� %# Wnb)/2�bPC	� M xM ? x? x? XZ\^->=_w� ��ya�mk+�d	�#mJ+
& 3yR, 3, a, �&  �5b%4�Ip)/4�)1\ �3&20pp' 3' ;ne"� lqlpiif l ~.;���(-1-b)%16~�S2cas�
#no'J
�  �!�) � � xE� Q0F 77�?mmsQ~`3��. �d�3)�P @zero�JS , hupper"�6>>CD�wA a bt!ofSAass_% �xfs, lenUf�6 	  �<M=2 f0fs+ Gor #�/: (of"�29ofs4 \   �2�2�0Uwhile_ <O  /Q Jap1)V4 7 '~=� w	� �Etx�ded API: creP/a z1con4! T�=z2tx:$ (��3N T  $,�y�9t5;5}
 6/ �6$ B  {V) - 1�4 �u�1io.�_'
 �4] � ) =G(
 	:�;%
 
�3 n
�i� �,&.DG2ctxj/64B�  x� /64� Q�  ��2#(a}1)\4 � #es��{0via ��  � [tO /64Q /64S %4	"=�=RID� 0reg  #(rlYr < 8��/%P nX[r-7]� c )64e /16f Qf 615]D3Tublic<~r 3s.
9 {:t,M  64 K %64 * � $ =
  &64  64`P� 8�Hjit_T����  2/  `�1s/s�cinglua Gjit/B ){sh�
L Ԁ� �        