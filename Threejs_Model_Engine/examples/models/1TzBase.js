(function (global, factory) {
	typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports) :
		typeof define === 'function' && define.amd ? define(['exports'], factory) :
			(factory((global.TZ = {})));
}(this, (function (exports) {
		'use strict';

		const {LineCurve3, Vector3, Shape, EllipseCurve, ExtrudeGeometry, Mesh, MeshBasicMaterial, MeshPhongMaterial, Color, Vector2, Geometry, Face3, Matrix4, CylinderGeometry, QuadraticBezierCurve3, TubeGeometry, Group} = THREE;


		class TzGeometry {

			///transform: [offsetX, offsetY, offsetZ, depth, rotate, axisX, axisY, axisZ]
			///convex:[type, ...data]
			// options: lod, grade, boundingbox
			constructor(id, color, options) {
				this.id = id;
				this.color = parseInt(color, 16);
				this.materialClass = MeshBasicMaterial;
				this.options = options;

				// arcgis用坐标系
				let newAxisX = new Vector3(0, 1, 0);
				let newAxisY = new Vector3(0, 0, 1);
				let newAxisZ = newAxisX.clone().cross(newAxisY);

				this.matrix = new Matrix4();
				this.matrix.makeBasis(newAxisX, newAxisY, newAxisZ);

			}

			set materialClass(v) {
				this._material = v;
			}

			get materialClass() {
				return this._material;
			}

			get material() {
				return new this.materialClass({color: this.color});
			}

			static set onAfterMeshMade(v) {
				TzGeometry._onAfterMeshMade = v;
			}

			static get onAfterMeshMade() {
				return TzGeometry._onAfterMeshMade;
			}

			static createInstance(instance) {
				return new Proxy(instance, {
					get(target, name) {
						const value = Reflect.get(target, name);
						if (name === 'mesh' && !target.__afterMeshMade__) {
							target.__afterMeshMade__ = true;
							TzGeometry.onAfterMeshMade(value);
						}
						return value;
					},
					set(target, p, value) {
						Reflect.set(target, p, value);
					},
				})
			}

		}

		TzGeometry.TYPE_EXTRUDE = 1;
		TzGeometry.TYPE_PIPE = 8;
		TzGeometry.TYPE_NORMAL = 2;
		TzGeometry.TYPE_BUFFERED = 3;
		TzGeometry.TYPE_CYLINDER = 4;
		TzGeometry.onAfterMeshMade = function () {
		};

		class TzExtrudeGeometry extends TzGeometry {

			///transform: [offsetX, offsetY, offsetZ, depth, rotate, axisX, axisY, axisZ]
			///convex:[type, ...data]
			constructor(lon, lat, id, transform, convex, holes, color, options) {
				super(id, color, options);
				this.lon = lon;
				this.lat = lat;
				this.transform = transform;
				this.convex = convex;
				this.holes = holes;
			}
			get mesh() {
				const [positionX, positionY, positionZ, depth, rotate, rotateX, rotateY, rotateZ, ] = this.transform;
				const axis = new Vector3(rotateX, rotateY, -rotateZ);
				axis.normalize();
				const shape = getShape(this.convex, false);
				if (this.holes.length)
					shape.holes.push(...this.holes.map(getShape));

				const extrudeSettings = {
					depth: Math.abs(depth),
					amount: Math.abs(depth),
					bevelEnabled: false,
				};
				const geometry = new ExtrudeGeometry(shape, extrudeSettings);
				const transform = new Matrix4();
				transform.makeRotationAxis(axis, rotate);
				const transform2 = new Matrix4();
				transform2.makeTranslation(positionX, positionY, positionZ);
				// geometry.rotateX(rotateX * Math.PI / 180);
				// geometry.rotateZ(-rotateY * Math.PI / 180);
				// geometry.translate(positionX, positionY, positionZ)

				// 坐标系变换
				// geometry.applyMatrix(transform);
				// geometry.applyMatrix(transform2);

				// 经纬度变换
				// geometry.rotateY(-this.lat / 180 * Math.PI);
				// geometry.rotateZ(this.lon / 180 * Math.PI);
				// geometry.ro

				const mesh = new Mesh(geometry);
				mesh.userData = this.options;
				if (axis.length() === 1) mesh.rotateOnAxis(axis, Math.PI / 180 * rotate);
				mesh.position.set(positionX, positionZ, -positionY);
				// mesh.rotateZ(this.lon / 180 * Math.PI);
				// mesh.rotateY(-this.lat / 180 * Math.PI);
				mesh.name = this.id;
				//钩子
				// this.onAfterMeshMade();
				return mesh;
			}

			static createInstance(lon, lat, id, transform, convex, holes, color, options) {
				return TzGeometry.createInstance(new TzExtrudeGeometry(lon, lat, id, transform, convex, holes, color, options));
			}

		}

		class TzCylinderGeometry extends TzGeometry {
			constructor(lon, lat, id, points, diameter, length, color, options) {
				super(id, color, options);
				this.lon = lon;
				this.lat = lat;
				this.points = points;
				this.diameter = diameter;
				this.length = length;
			}

			get mesh() {
				let [startP, endP] = this.points.map(arr => new Vector3(...arr));
				if (startP.z < endP.z) {
					let temp = endP;
					endP = startP;
					startP = temp;
				}
				let path = new LineCurve3(startP, endP);
				let geometry = new TubeGeometry(path, 2, this.diameter / 2, 8, true);
				// 坐标系变换
				geometry.applyMatrix(this.matrix);

				// 经纬度变换
				geometry.rotateY(-this.lat / 180 * Math.PI);
				geometry.rotateZ(this.lon / 180 * Math.PI);

				let mesh = new Mesh(geometry, this.material);
				mesh.userData = this.options;
				mesh.name = this.id;
				return mesh;
			}

			static createInstance(lon, lat, id, points, diameter, length, color, options) {
				return TzGeometry.createInstance(new TzCylinderGeometry(lon, lat, id, points, diameter, length, color, options));
			}

			static from(lon, lat, ids, points, diameter, length, colors, options) {
				let objects = [];
				for (let i = 0; i < points.length; i += 2) {
					let startP = points[i];
					let endP = points[i + 1];
					let id = ids[i / 2];
					objects.push(TzGeometry.createInstance(new TzCylinderGeometry(lon, lat, id, [startP, endP], diameter, length, colors[i / 2], options)));
				}
				return objects;
			}
		}

		class TzIrregularGeometry extends TzGeometry {

			///transform: [offsetX, offsetY, offsetZ, depth, rotate, axisX, axisY, axisZ]
			///convex:[type, ...data]
			constructor(lon, lat, id, points, facets, normals, color, options) {
				super(id, color, options);
				this.lon = lon;
				this.lat = lat;
				this.points = points;
				this.facets = facets;
				this.normals = normals;
			}

			get mesh() {
				const geometry = generateGeometry(this.points, this.facets, this.normals);

				// 坐标系变换
				// geometry.applyMatrix(this.matrix);

				// 经纬度变换
				geometry.rotateY(-this.lat / 180 * Math.PI);
				geometry.rotateZ(this.lon / 180 * Math.PI);

				const mesh = new Mesh(geometry, this.material);
				mesh.userData = this.options;
				//默认name
				mesh.name = this.id;
				return transformToThree(mesh);
			}

			static createInstance(lon, lat, id, points, facets, normals, color, options) {
				return TzGeometry.createInstance(new TzIrregularGeometry(lon, lat, id, points, facets, normals, color, options));
			}
		}


		class TzIrregularBufferedGeometry extends TzGeometry {
			constructor(lon, lat, symbolId, instanceId, points, facets, normals, transform, color, options) {
				super(symbolId, color, options);
				this.lon = lon;
				this.lat = lat;
				this.points = points;
				this.facets = facets;
				this.normals = normals;
				this.transform = transform;
				this.instanceId = instanceId;
			}

			get mesh() {
				if (!TzIrregularBufferedGeometry.buffer) TzIrregularBufferedGeometry.buffer = new Map();
				if (!TzIrregularBufferedGeometry.buffer.has(this.id)) {
					const geometry = generateGeometry(this.points, this.facets, this.normals);
					TzIrregularBufferedGeometry.buffer.set(this.id, geometry);
				}

				const geometry = TzIrregularBufferedGeometry.buffer.get(this.id);
				// geometry.rotateY(Math.PI / 2);
				// geometry.rotateX(Math.PI / 2);
				geometry.rotateY(-this.lat / 180 * Math.PI);
				geometry.rotateZ(this.lon / 180 * Math.PI);

				let mesh = new Mesh(geometry, this.material);
				mesh.userData = this.options;
				mesh.name = this.id;
				transformToThree(mesh);
				applyRawMatrix(mesh, this.transform);
				return mesh;
			}

			static createInstance(lon, lat, symbolId, instanceId, points, facets, normals, transform, color, options) {
				return TzGeometry.createInstance(new TzIrregularBufferedGeometry(lon, lat, symbolId, instanceId, points, facets, normals, transform, color, options));
			}
		}


		class TzPipeGeometry extends TzGeometry {

			///transform: [offsetX, offsetY, offsetZ, depth, rotate, axisX, axisY, axisZ]
			///convex:[type, ...data]
			constructor(id, vertices, radius, elbows, colorIdx) {
				super(id, colorIdx);
				this.vertices = vertices;
				this.radius = radius;
				this.elbows = elbows;
			}

			//drawPipe(id, vertices, radius, elbows, color);
			// function drawPipe(id, data, inRadius, inElbow, color = 0xffdd00) {
			get mesh() {
				const group = new Group();

				const vertices = [];
				for (let i = 0; i < this.vertices.length; i += 3) {
					let [x, y, z] = this.vertices.slice(i);
					vertices.push(new Vector3(x, z, -y));
				}

				const elbow = [0];
				for (let i = 0; i < this.elbows.length; i += 2) {
					let [l, arr] = this.elbows.slice(i);
					arr.forEach(n => elbow[n] = l);
				}

				const radiusArr = [];
				for (let i = 0; i < this.radius.length; i += 2) {
					let [r, arr] = this.radius.slice(i);
					arr.forEach(n => radiusArr[n] = r);
				}

				for (let i = 0; i < vertices.length; i++) {
					if (radiusArr[i] === undefined) radiusArr[i] = radiusArr[i - 1];
				}
			}

			static createInstance(id, vertices, radius, elbows, colorIdx) {
				return TzGeometry.createInstance(new TzPipeGeometry(id, vertices, radius, elbows, colorIdx));
			}

		}


// export {TzIrregularGeometry, TzIrregularBufferedGeometry};

		const getShape = (vertices, inner = true) => {
			let type = vertices[0];
			const shape = new Shape();
			if (type === 1) {
				shape.moveTo(vertices[1], vertices[2]);
				for (let i = 3; i < vertices.length; i += 2) {
					shape.lineTo(vertices[i], vertices[i + 1]);
				}
				shape.closePath();
				return shape;
			} else if (type === 2) {
				const [_, radius, x, y] = vertices;
				const shape = new EllipseCurve(x, y, radius, radius, 0, 2 * Math.PI, inner, 0);
				return new Shape(shape.getPoints(32));
			} else {
				shape.moveTo(vertices[1][1], vertices[1][2]);
				shape.autoClose = false;
				for (let i = 1; i < vertices.length; i++) {
					const [t, x1, y1, cx, cy, r, s, e, ccw] = vertices[i];
					//x1,y1,x2,y2...
					//[1,x1,y1],[1,x2,y2],[2,x3,y3,cx, cy, r, s, e, clockwise],[1,x4,y4]....
					// if (t === 2) {
					// 	shape.absarc(cx, cy, r, s, e, ccw === 1);
					// } else {
						shape.lineTo(x1, y1);
					// }
				}
				return shape;
			}
		};

		const drawCylinder = (topCenter, bottomCenter, topRadius, bottomRadius, material) => {
			const basisY = new Vector3(0, 1, 0);
			//底指向顶
			const direction = topCenter.clone().sub(bottomCenter).normalize();
			const height = topCenter.clone().sub(bottomCenter).length();
			const geometry = new CylinderGeometry(topRadius, bottomRadius, height, 16);
			const cylinder = new Mesh(geometry, material);

			const axis = direction.clone().cross(basisY).normalize();
			const angle = direction.angleTo(basisY);

			const center = topCenter.clone().add(bottomCenter).divideScalar(2);
			cylinder.translateOnAxis(center.clone().normalize(), center.length());
			if (angle > 0) cylinder.rotateOnAxis(axis, -angle);
			return cylinder;
		};

		const drawElbowPipe = (startPoint, controlPoint, endPoint, radius, material) => {
			const path = new QuadraticBezierCurve3(startPoint, controlPoint, endPoint);
			const geometry = new TubeGeometry(path, 32, radius, 8, false);
			return new Mesh(geometry, material);
		};


		const normalize = arr => arr.map(item => item / Math.hypot(...arr));

		const normalizeMatrix = matrix => {
			let o = [];
			for (let i = 0; i < matrix.length; i += 3) {
				o = [...o, ...normalize(matrix.slice(i, 3))]
			}
			return o;
		};

		const generateGeometry = (inPoints, inFacets, inNormals) => {
			const facets = split(inFacets, 3);
			const normals = splitNormals(inNormals);
			const geometry = new Geometry();

			// geometry.uvsNeedUpdate = true;
			// geometry.elementsNeedUpdate = true;
			// geometry.normalsNeedUpdate = true;

			inPoints.forEach(point => geometry.vertices.push(new Vector3(...point)));
			facets.forEach(facet => geometry.faces.push(new Face3(...facet, normals[facet[0]])));
			assignUVs(geometry, inPoints);

			// geometry.computeBoundingSphere();
			// geometry.computeBoundingBox();

			// geometry.uvsNeedUpdate = false;
			// geometry.elementsNeedUpdate = false;
			// geometry.normalsNeedUpdate = false;
			return geometry;
		};

		const transformToThree = mesh => {
			// mesh.matrixAutoUpdate = false;
			// let matrix = new Matrix4();
			// // //revit => three 坐标轴变换
			// matrix.set(
			//     0, 1, 0, 0,
			//     0, 0, 1, 0,
			//     1, 0, 0, 0,
			//     0, 0, 0, 1);
			// mesh.matrix.multiply(matrix);
			return mesh;
		};

		const applyRawMatrix = (mesh, inMatrix) => {
			let matrix4 = new Matrix4();
			const matrix = normalizeMatrix(inMatrix.slice(0, 9));
			const [x, y, z] = inMatrix.slice(9);

			matrix4.set(matrix[0], matrix[3], matrix[6], 0,
				matrix[1], matrix[4], matrix[7], 0,
				matrix[2], matrix[5], matrix[8], 0,
				0, 0, 0, 1);

			mesh.matrix.multiply(matrix4).setPosition(new Vector3(x, z, -y));
			return mesh;
		};

		const assignUVs = (geometry, points) => {
			let maxU, minU, maxV, minV;
			maxU = maxV = Number.MIN_VALUE;
			minU = minV = Number.MAX_VALUE;
			points.forEach(ele => {
				if (maxU < ele[0]) maxU = ele[0];
				if (minU > ele[0]) minU = ele[0];

				if (maxV < ele[1]) maxV = ele[1];
				if (minV > ele[1]) minV = ele[1];
			});
			let scaleU = maxU - minU;
			let scaleV = maxV - minV;
			const newUvs = points.map(uv => new Vector2((uv[0] - minU) / scaleU, (uv[1] - minV) / scaleV));

			const faces = geometry.faces;
			geometry.faceVertexUvs[0] = [];
			for (let i = 0; i < faces.length; i++) {
				geometry.faceVertexUvs[0].push([
					newUvs[faces[i].a],
					newUvs[faces[i].b],
					newUvs[faces[i].c]
				]);
			}
		};

		const split = (arr, cnt) => {
			let output = [];
			for (let i = 0; i < arr.length; i += cnt) {
				output.push(arr.slice(i, i + cnt));
			}
			return output;
		};

		const splitNormals = normals => {
			let output = [];
			for (let i = 0; i < normals.length; i++) {
				output.push(new Vector3(normals[i][1], normals[i][2], normals[i][3]));
			}
			return output;
		};

		const fromAxisAngleTranslate = (axis, angle, translate) => {
			let [x, y, z] = axis;
			let [px, py, pz] = translate;
			let c = Math.cos(angle), s = Math.sin(angle), cc = 1 - c;
			return new Matrix4().fromArray([c + x * x * cc, -s * z + cc * x * y, s * y + cc * x * z, 0,
				s * z + cc * x * y, c + y * y * cc, -s * x + cc * y * z, 0,
				-s * y + cc * x * z, s * x + cc * y * z, c + cc * z * z, 0,
				px, py, pz, 1]);
		};

		const fromAxisAngleTranslateInverse = (axis, angle, translate) => {
			let [x, y, z] = axis;
			let [px, py, pz] = translate;
			let c = Math.cos(angle), s = Math.sin(angle), cc = 1 - c;
			return new Matrix4().fromArray([c + x * x * cc, s * z + cc * x * y, -s * y + cc * x * z, px,
				-s * z + cc * x * y, c + y * y * cc, s * x + cc * y * z, py,
				s * y + cc * x * z, -s * x + cc * y * z, c + cc * z * z, pz,
				0, 0, 0, 1]);
		};

		exports.TzGeometry = TzGeometry;
		exports.TzExtrudeGeometry = TzExtrudeGeometry;
		exports.TzIrregularGeometry = TzIrregularGeometry;
		exports.TzIrregularBufferedGeometry = TzIrregularBufferedGeometry;
		exports.TzPipeGeometry = TzPipeGeometry;
		exports.TzCylinderGeometry = TzCylinderGeometry;

		Object.defineProperty(exports, '__esModule', {value: true});


	}
)));

