快速上手：

	1.	安装VS2012环境安装包(vcredist_x86_vs2012.exe)、VS2013环境安装包（vcredist_x86_vs2013.exe）
	
	2.	从官网申请sdk  http://www.arcsoft.com.cn/ai/arcface.html  ，下载对应的sdk版本(x86或x64)并解压
	
	3.	将libs中的“libarcsoft_face.dll”、“libarcsoft_face_engine.dll” 、“libarcsoft_idcardveri.dll”拷贝到工程bin目录的对应平台的debug或release目录下
	
	4.	将对应appid和appkey替换App.config文件中对应内容
	
	5.	在Debug或者Release中选择配置管理器，选择对应的平台
	
	6.	按F5启动程序
	
	7.	查看文本框相关信息 


常见问题：

	1.后引擎初始化失败	
		(1)请选择对应的平台，如x64,x86 
		(2)删除bin下面对应的idv_install.dat；
		(3)请确保App.config下的appid，和appkey与当前sdk一一对应。
		
		
	2.使用人脸检测功能对图片大小有要求吗？	
		推荐的图片大小最大不要超过2M，因为图片过大会使人脸检测的效率不理想，当然图片也不宜过小，否则会导致无法检测到人脸。
		
	3.使用人脸识别引擎提取到的人脸特征信息是什么？	
		人脸特征信息是从图片中的人脸上提取的人脸特征点，是byte[]数组格式。 
		
	4.SDK人脸比对的阈值设为多少合适？	
		推荐值为0.82，用户可根据不同场景适当调整阈值。
		
	5.可不可以将人脸特征信息保存起来，等需要进行人脸比对的时候直接拿保存好的人脸特征进行比对？
		可以，当人脸个数比较多时推荐先存储起来，在使用时直接进行比对，这样可以大大提高比对效率。存入数据库时，请以Blob的格式进行存储，不能以string或其他格式存储。