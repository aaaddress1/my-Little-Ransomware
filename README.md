# my-Little-Ransomware

> 這是 (SITCON 2015)[http://sitcon.org/2015/] 上針對勒索軟體演講的一個開源時做驗證 PoC

一個以CSharp實作簡單模擬勒索軟體（基於Windows），提供了範例實作：<br>
* AES加密、解密文件
* 開機自啟動
* RSA加密、解密對稱金鑰
* 隨機化加密文件之檔名（恢復時可正確還原成正確檔名）

