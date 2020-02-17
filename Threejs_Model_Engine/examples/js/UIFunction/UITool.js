/**
 * @author alteredq / http://alteredqualia.com/
 * @author mr.doob / http://mrdoob.com/
 */

var UITool = {

	//给树状图增加节点
	TreeAddChild:function(a){

		if(!a){return '';}  //当前节点不存在的时候，退出
		var html='\n<ul >\n';
		for(var i=0;i<a.length;i++){
			html+='<li>'+a[i].name;
			html+=this.TreeAddChild(this.groups[a[i].id]);
			html+='</li>\n';
		};
		html+='</ul>\n';
		return html;
	},


	DomainTreeInit:function(data,html){

		if(data.Children.length>0){

			html.content += "<div class='cbox-treenodeNoexp'  id='" + data.DomainName + "' treetype='1'  selected='0' show='1' >";
			html.content += "<img  src='files/img/btn_tips_2.png'  class=\"cbox-treearrow\" onclick='DomainArrowOnClick(this.parentNode)' >";
			html.content += "<img  src='files/img/CheckBox_Normal.png'  class=\"cbox-treebox\" onclick='DomainBoxOnClick(this.parentNode)' >";
			html.content += "<label class=\"cbox-treelabel\" >" + data.DomainName+"</label>";
			for (var i=0;i<data.Children.length;i++){
				this.DomainTreeInit(data.Children[i],html);
			}
			html.content+=" </div>";
		}else{
			html.content += "<div class='cbox-treenodeNoexp'  id='" + data.DomainName + "' treetype='0'  selected='0'  show='1' >";
			html.content += "<img  src='files/img/CheckBox_Normal.png'  class=\"cbox-treebox\" onclick='DomainBoxOnClick(this.parentNode)' >";
			html.content += "<label class=\"cbox-treelabel\" >" + data.DomainName+"</label>";
			html.content +=" </div>";
		}

	},


	UpdateTreeInit:function(data,html){


			if(data.children && data.children.length>0){
				html.content += "<div class='cbox-treenodeNoexp'  id='" + data.Name + "' treetype='1'  selected='0' show='1' >";
				html.content += "<img  src='files/img/btn_tips_2.png'  class=\"cbox-treearrow\" onclick='DomainArrowOnClick(this.parentNode)' >";
				html.content += "<img  src='files/img/CheckBox_Normal.png'  class=\"cbox-treebox\" onclick='DomainBoxOnClick(this.parentNode)' >";
				html.content += "<label class=\"cbox-treelabel\" >" + data.Name+"</label>";
				for (var i=0;i<data.children.length;i++){
					this.UpdateTreeInit(data.children[i],html);
				}

				html.content+=" </div>";
			}else{
				html.content += "<div class='cbox-treenodeNoexp'  id='" + data.Name + "' treetype='0'  selected='0'  show='1' >";
				html.content += "<img  src='files/img/CheckBox_Normal.png'  class=\"cbox-treebox\" onclick='DomainBoxOnClick(this.parentNode)' >";
				html.content += "<label class=\"cbox-treelabel\" >" + data.Name+"</label>";
				html.content +=" </div>";
			}
	},


	TreeStatusChange(data,code){
		if(data.hasAttribute("treetype")){
			var treetype=data.getAttribute("treetype");
			var img="";
			if(code==0){
				img="files/img/CheckBox_Normal.png";

			}else if(code==1){
				img="files/img/CheckBox_Checked_Part.png";

			}else if(code==2){
				img="files/img/CheckBox_Checked.png";
			}

			if(treetype==0){
				data.childNodes[0].src=img;
			}else{
				data.childNodes[1].src=img;
			}
			data.setAttribute("selected",code);
		}

	},

	TreeParentNodeChange(data){
		//判断是否有上一层 如果有就改变上一层的状态
		if(data.parentNode.hasAttribute("treetype")) {
			var nodes= data.parentNode.childNodes;
			var selectnumber=0;
			var allnumbers=0;
			var partnumbers=0;
			for (var i=0;i<nodes.length;i++){
				if(nodes[i].tagName=="DIV"){
					allnumbers++;
					if(nodes[i].getAttribute("selected")==2 ){
						selectnumber++;
					}else if(nodes[i].getAttribute("selected")==1){
						partnumbers++;
					}
				}
			}
			var newcode=0;

			if(partnumbers>0){
				newcode=1;
			}else{
				if(selectnumber==allnumbers){
					newcode=2;
				}else if(selectnumber==0){
					newcode=0;
				}else if(selectnumber>0){
					newcode=1;
				}
			}
			this.TreeStatusChange(data.parentNode,newcode);
			this.TreeParentNodeChange(data.parentNode);
		}
	},

	TreeChildChange(data,code){
		var childarray= data.childNodes;
		for (var i=0;i<childarray.length;i++){
			if(childarray[i].tagName=="DIV"){
				if(childarray[i].getAttribute("selected")!=code){
					this.TreeStatusChange(childarray[i],code);
					this.TreeChildChange(childarray[i],code);
				}
			}
		}
	},


	TreeChangeOnClick(data){
		var selected=data.getAttribute("selected");
		var newcode="";

		if(selected==0){
			newcode=2;
		}else if(selected==1){
			newcode=2;
		}else if(selected==2){
			newcode=0;
		}
		this.TreeStatusChange(data,newcode);
		this.TreeChildChange(data,newcode);
		this.TreeParentNodeChange(data);


	},


	TreeAdd:function(json,div){
		var html="";
		var data= JSON.parse(json);
		if(data.length > 0){
			for (var i=0;i<data.length;i++){
				this.TreeAddChild(div,data[i]);
			}
		}
		return html;
	},



	RequestGet:function(url,even){
		var httpRequest = new XMLHttpRequest();//第一步：建立所需的对象
		httpRequest.open('GET', url, true);//第二步：打开连接  将请求参数写在url中  ps:"./Ptest.php?name=test&nameone=testone"
		httpRequest.send();//第三步：发送请求  将请求参数写在URL中
		/**
		 * 获取数据后的处理程序
		 */
		httpRequest.onreadystatechange = function () {
			if (httpRequest.readyState == 4 && httpRequest.status == 200) {
				var json = httpRequest.responseText;//获取到json字符串，还需解析
				even(json);
				console.log(json);
			}else{

				console.log(httpRequest.readyState+">>>"+httpRequest.status+">>");

			}
		};
	},

	RequestPost:function(url,data,even){
		var httpRequest = new XMLHttpRequest();//第一步：创建需要的对象
		httpRequest.open('POST', url, true); //第二步：打开连接
		httpRequest.setRequestHeader("Content-type","application/json");//设置请求头 注：post方式必须设置请求头（在建立连接后设置请求头）
		httpRequest.send(data);//发送请求 将情头体写在send中
		/**
		 * 获取数据后的处理程序
		 */
		httpRequest.onreadystatechange = function () {//请求后的回调接口，可将请求成功后要执行的程序写在其中
			if (httpRequest.readyState == 4 && httpRequest.status == 200) {//验证请求是否发送成功
				var json = httpRequest.responseText;//获取到服务端返回的数据
				console.log(json);
				even(json);
			}else{
				console.log(httpRequest.readyState+">>>"+httpRequest.status+">>");
			}
		};
	},

	RequestPostJson:function(url,json){
		var httpRequest = new XMLHttpRequest();//第一步：创建需要的对象
		httpRequest.open('POST', url, true); //第二步：打开连接/***发送json格式文件必须设置请求头 ；如下 - */
		httpRequest.setRequestHeader("Content-type","application/json");//设置请求头 注：post方式必须设置请求头（在建立连接后设置请求头）var obj = { name: 'zhansgan', age: 18 };
		httpRequest.send(JSON.stringify(json));//发送请求 将json写入send中
		/**
		 * 获取数据后的处理程序
		 */
		httpRequest.onreadystatechange = function () {//请求后的回调接口，可将请求成功后要执行的程序写在其中
			if (httpRequest.readyState == 4 && httpRequest.status == 200) {//验证请求是否发送成功
				var json = httpRequest.responseText;//获取到服务端返回的数据
				console.log(json);
			}
		};

	},


	isWebGL2Available: function () {

		try {

			var canvas = document.createElement( 'canvas' );
			return !! ( window.WebGL2RenderingContext && canvas.getContext( 'webgl2' ) );

		} catch ( e ) {

			return false;

		}

	},

	getWebGLErrorMessage: function () {

		return this.getErrorMessage( 1 );

	},

	getWebGL2ErrorMessage: function () {

		return this.getErrorMessage( 2 );

	},

	getErrorMessage: function ( version ) {

		var names = {
			1: 'WebGL',
			2: 'WebGL 2'
		};

		var contexts = {
			1: window.WebGLRenderingContext,
			2: window.WebGL2RenderingContext
		};

		var message = 'Your $0 does not seem to support <a href="http://khronos.org/webgl/wiki/Getting_a_WebGL_Implementation" style="color:#000">$1</a>';

		var element = document.createElement( 'div' );
		element.id = 'webglmessage';
		element.style.fontFamily = 'monospace';
		element.style.fontSize = '13px';
		element.style.fontWeight = 'normal';
		element.style.textAlign = 'center';
		element.style.background = '#fff';
		element.style.color = '#000';
		element.style.padding = '1.5em';
		element.style.width = '400px';
		element.style.margin = '5em auto 0';

		if ( contexts[ version ] ) {

			message = message.replace( '$0', 'graphics card' );

		} else {

			message = message.replace( '$0', 'browser' );

		}

		message = message.replace( '$1', names[ version ] );

		element.innerHTML = message;

		return element;

	}

};
