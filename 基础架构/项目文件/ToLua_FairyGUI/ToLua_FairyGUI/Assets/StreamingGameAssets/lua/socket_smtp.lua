UnityFS    5.x.x 5.3.6f1       �   A   [   C  �  0H       � CAB-bd05d0badb6d03c278574de569a6108b �  �  0H       �5.3.6f1       1   �g9�_&�����p���� ! �    O �7  �����B � � H ��  (  s1  �10  `  @ �  �  � �   \   �Q  �j  �  ` �` ` ` ` ` H `  `  �  H 	� 
� �  �m_PathName �   򄫶�����+kPCh,   �  _a@� �� _  _ _ "y` _ 0 t 	x s . �$, 
� S�  �-� 
�  
� � �  0 0 P�  ` P` � P� 9� � �   � H C P   P\K P� PP � bt �	�  �  � 0 p� � S� �n0 H ��� �!�"�#��` $` %X&� &HX'X	H (� H )� � *� SL  �� +` �@  AssetBundleS�reloadTab cFileIDk	 �Container9 bInfo p5 TIndex `Size a# � m_MainA �RuntimeCompatibility m_� ��m_Dependencies m_IsStreamedScene� # �  1�  "� ��a��F�G��   ��1  ���������6 �� socket_smtp.lua�	x	  2/  
1s/s�	dinglua B /B c.bytes
L � �   , _�  - 9�
-- SMTP client support for the Lua language.- ALuaS� � toolkit �Author: Diego Nehab
� ;
� >�Declare module and im� d�x <� local base = _G �coroutin �require(" #")' hstring$  ! Hmath    (os &osW �W  ! .tp 5.tp  Xltn12#   headersD   � )im  C")

/ !sm� #{}/ W_M =  %@�Program constantsc ? U"ous�connection
_M.TIMEOUT = 60* �default server used to send e-mails7 �SERVER = "'Xhost"?  � ( 0POR\ !25 RomainT �in HELO comm  |  ndj  4 `If we D�under a CGI, try� �get from environmenw �DOMAIN = os.getenv("� �_NAME") or�  ,� zone (means� �don't know)_ @ZONE� �-0000"

�<�Low level7?API"B@meta�P{ __i��q }

fun!# . r:greet(�� )
    self.try(	 �p:check("2..")# ��("EHLO",5or �; ereturn~okip(1,t _)
end�  V(1� �MAIL", "FROM:" ..g� � � rcpt(to� @RCPT� "TO� /to� 6�data(src, step� oDATA")�3�gsourcep ( en P\r\n. "� 6_quit(� OQUITs 7oclose(N  p ? 	�login(user, password� @AUTH0_LOGIN�* � A.b64� )��X 7� \ �/?pla?�!au�0"PL)^� 1"\0  > �  � c � 6 M � R, ext` bif not� !or &  cn�  1 d 2 �	�.find(ext, � c[^\n]+B= 0 � �Oelse^ u^ 	�^ 3 �`nil, "$aenticaA%nol3ed"? "ndg"--��message ��row an excep!	��Failt\ .b!if�S.type  �p) == "tI" �	0i,v 	9 iipairs; %do. } Q%v) �/* O 
/ " _.�0.ch�5  ,.�stuff()),  L
��_M.open(R
,
�, create�&tpj �3tp.�
; #{
H   f
,� �
g h  %[1set� �!({ Btp},�2 �-- make sure� 1is �1d i�
 �
	1E � � 0new� &()�	$:end-�uconvert��to lowercase�	n _* ( � ! �poj "or0 	u [� �(i)] = vL� ! � ':�Multipart��??� �s a hopefully uniqueO�boundary��eqno = 0�4new+ �+  9+ 1C�ormat('%s%05d==%05u',��date('%d%m%Y%H%M%S') ��.random(0, 9 "),k )_A`forwar� �G� ' 8 Ayiel]��all at on�it's fasterK � to ��canonic =Y . $ �1�f�[ � 8 (O 0[f]�f~@': ' v� 
 h�..� &(hFm�Bbody�a  {, _(mesg��we have our�F�;3Obd =� ?
@} ."{}7  �['content-�' Eor '� /mixed'K $%..�%';� !="� �  �"^ 	m	pp ���  �. 
�1  )- �
	X� Qeach !0sepved by a`�- m#� 	�� !--�+� 3�/(m� Jlast� h  -- n � opilogu� �1 ��� X���(a �z]/Qtext/^�; charset="iso-8859-15  0whi,:rue/��hunk, er|�	!if ���7err�
fW 4  
1 f break��� ���&�	�rK
>�>4(G � J �>�PtE5jadjust��
&)
�" 
"�
 �%
�
"!%a, %d %b %Y %H:%M:%S "�c �R �	k x�(ero  $or��_VERSION2 th5a@�be overriden\  DP-vers� ` _"1.0"EX3 
;=s#--6	i �� ��: ��� ^ �ret, a, be �resume(co%9ret� 7 
� 5a��<OHigh�K1_M.xS@protP��y��� � exs�-	�� 'usW �s.�3)

 �_M      