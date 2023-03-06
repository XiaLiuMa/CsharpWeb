//  input 数据校验
export default {
    validatePhone,
    // 下拉空校验
    selectNotNull(info) {
        return [{
            required: true, message: '请选择' + info, trigger: 'change'
        }]
    },
    // 输入空校验
    importNotNull(info) {
        return [{
            required: true, message: '请输入' + info, trigger: 'change'
        }]
    },
    uploadNotNull(info) {
        return [{
            required: true, message: '请上传' + info, trigger: 'change'
        }]
    },
    // 上传空校验
    // 第1校验 校验IP地址
    checkIP: [{ required: true, validator: validateIP, trigger: 'blur' }],
    // 第2校验 校验手机号/固话
    checkPhoneTwo: [{ required: true, validator: validatePhoneTwo, trigger: 'blur' }],
    // 第3校验 校验固话
    checkTelephone: [{ required: true, validator: validateTelphone, trigger: 'blur' }],
    // 第4校验 校验手机号
    checkPhone: [{ required: true, validator: validatePhone, trigger: 'blur' }],
    // 第5校验 校验身份证号码
    checkIDNum: [{ required: true, validator: validateIdNo, trigger: 'blur' }],
    // 第6校验 校验邮箱
    checkEMail: [{ required: true, validator: validateEMail, trigger: 'blur' }],
    // 第7校验 校验url地址
    checkURL: [{ required: true, validator: validateURL, trigger: 'blur' }],
    // 第8校验 校验英文/符号
    checkSymbol: [{ required: true, validator: isPassword, trigger: 'blur' }],
    // 第9校验 校验自动检验的数值范围
    checkMaxAuto: [{ required: true, validator: checkMax20000, trigger: 'blur' }],
    // 第10校验 校验最大值
    checkMaxVal: [{ required: true, validator: checkMaxVal, trigger: 'blur' }],
    // 第11校验 校验1~99
    checkOneToNinetyNine: [{ required: true, validator: isOneToNinetyNine, trigger: 'blur' }],
    // 第12校验 校验整数
    checkInteger: [{ required: true, validator: isInteger, trigger: 'blur' }],
    // 第13校验 校验整数非必填
    checkIntegerNoMust: [{ required: true, validator: isIntegerNotMust, trigger: 'blur' }],
    // 第14校验 校验0~1小数
    checkZeroToOne: [{ required: true, validator: isDecimal, trigger: 'blur' }],
    // 第15校验 校验1~10小数 即不可以等于0
    checkOneToTen: [{ required: true, validator: isBtnOneToTen, trigger: 'blur' }],
    // 第16校验 校验1~100小数 既不可以等于0
    checkOneToHundredNotZero: [{ required: true, validator: isBtnOneToHundredNotZero, trigger: 'blur' }],
    // 第17校验 校验1~100小数
    checkOneToHundred: [{ required: true, validator: isBtnOneToHundred, trigger: 'blur' }],
    // 第18校验 端口是否在[0,65535]之间
    checkIsPort: [{ required: true, validator: isPort, trigger: 'blur' }],
    // 第19校验 端口是否在[0,65535]之间，非必填,isMust表示是否必填
    checkPort: [{ required: true, validator: isCheckPort, trigger: 'blur' }],
    // 第20校验 校验小写字母
    checkLowerCase: [{ required: true, validator: validateLowerCase, trigger: 'blur' }],
    // 第21校验 校验最多两位小数
    checkTwoPoint: [{ required: true, validator: validateValidity, trigger: 'blur' }],
    // 第22校验 校验大写字母
    checkUpperCase: [{ required: true, validator: validateUpperCase, trigger: 'blur' }],
    // 第23校验 校验大小写字母
    checkCase: [{ required: true, validator: validateAlphabets, trigger: 'blur' }],
    // 第24校验 校验密码
    checkPsd: [{ required: true, validator: validatePsdReg, trigger: 'blur' }],
    // 第25校验 校验中文
    checkChinese: [{ required: true, validator: validateContacts, trigger: 'blur' }],
    // 第26校验 校验身份证
    checkID: [{ required: true, validator: ID, trigger: 'blur' }],
    // 第27校验 校验账号
    checkAccount: [{ required: true, validator: validateCode, trigger: 'blur' }],
    // 第28校验 校验纯数字
    checkNumber: [{ required: true, validator: validateNumber, trigger: 'P' }],
    // 第29校验 校验最多一位小数
    onePoint: [{ required: true, validator: onePoint, trigger: 'blur' }]
}

// 2、是否手机号码或者固话
export function validatePhoneTwo(rule, value, callback) {
    const reg = /^((0\d{2,3}-\d{7,8})|(1[34578]\d{9}))$/
    if (value === '' || value === undefined || value == null) {
        callback()
    } else {
        if ((!reg.test(value)) && value !== '') {
            callback(new Error('请输入正确的电话号码或者固话号码'))
        } else {
            callback()
        }
    }
}

// 3、是否固话
export function validateTelphone(rule, value, callback) {
    const reg = /0\d{2,3}-\d{7,8}/
    if (value === '' || value === undefined) {
        callback()
    } else {
        if ((!reg.test(value)) && value !== '') {
            callback(new Error('请输入正确的固定电话）'))
        } else {
            callback()
        }
    }
}

// 4、是否手机号码
export function validatePhone(rule, value, callback) {
    const reg = /^[1][3-9][0-9]{9}$/
    if (value === '' || value === undefined || value == null) {
        callback(new Error('手机号不能为空'))
    } else {
        if ((!reg.test(value)) && value !== '') {
            callback(new Error('请输入正确的电话号码'))
        } else {
            callback()
        }
    }
}

// 5、是否身份证号码
export function validateIdNo(rule, value, callback) {
    const reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/
    if (value === '' || value === undefined || value == null) {
        callback(new Error('身份证号码不能为空'))
    } else {
        if ((!reg.test(value)) && value !== '') {
            callback(new Error('请输入正确的身份证号码'))
        } else {
            callback()
        }
    }
    //if (value === '' || value === undefined || value == null) {
    //    callback(new Error('身份证号码不能为空'))
    //} else {
    //    if (!checkCard(value)) {
    //        callback(new Error('请输入正确的身份证号码'))
    //    } else {
    //        callback()
    //    }
    //}
}

// 6、是否邮箱
export function validateEMail(rule, value, callback) {
    const reg = /^([a-zA-Z0-9]+[-_\\.]?)+@[a-zA-Z0-9]+\.[a-z]+$/
    if (value === '' || value === undefined || value == null) {
        callback()
    } else {
        if (!reg.test(value)) {
            callback(new Error('请输入正确的邮箱'))
        } else {
            callback()
        }
    }
}

// 7、合法url
export function validateURL(url) {
    const urlregex = /^(https?|ftp):\/\/([a-zA-Z0-9.-]+(:[a-zA-Z0-9.&%$-]+)*@)*((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])){3}|([a-zA-Z0-9-]+\.)*[a-zA-Z0-9-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(:[0-9]+)*(\/($|[a-zA-Z0-9.,?'\\+&%$#=~_-]+))*$/
    return urlregex.test(url)
}

// 8、验证内容是否包含英文数字以及下划线
export function isPassword(rule, value, callback) {
    const reg = /^[_a-zA-Z0-9]+$/
    if (value === '' || value === undefined || value == null) {
        callback()
    } else {
        if (!reg.test(value)) {
            callback(new Error('仅由英文字母，数字以及下划线组成'))
        } else {
            callback()
        }
    }
}

// 9、自动检验数值的范围
export function checkMax20000(rule, value, callback) {
    if (value === '' || value === undefined || value == null) {
        callback()
    } else if (!Number(value)) {
        callback(new Error('请输入[1,20000]之间的数字'))
    } else if (value < 1 || value > 20000) {
        callback(new Error('请输入[1,20000]之间的数字'))
    } else {
        callback()
    }
}

// 10、验证数字输入框最大数值
export function checkMaxVal(rule, value, callback) {
    // 最大值为10
    if (value < 0 || value > 10) {
        callback(new Error('请输入[0,最大值]之间的数字'))
    } else {
        callback()
    }
}

// 11、验证是否1-99之间
export function isOneToNinetyNine(rule, value, callback) {
    if (!value) {
        return callback(new Error('输入不可以为空'))
    }
    setTimeout(() => {
        if (!Number(value)) {
            callback(new Error('请输入正整数'))
        } else {
            const re = /^[1-9][0-9]{0,1}$/
            const rsCheck = re.test(value)
            if (!rsCheck) {
                callback(new Error('请输入正整数，值为【1,99】'))
            } else {
                callback()
            }
        }
    }, 0)
}

// 12、验证是否整数
export function isInteger(rule, value, callback) {
    if (!value) {
        return callback(new Error('输入不可以为空'))
    }
    setTimeout(() => {
        if (!Number(value)) {
            callback(new Error('请输入正整数'))
        } else {
            const re = /^[0-9]*[1-9][0-9]*$/
            const rsCheck = re.test(value)
            if (!rsCheck) {
                callback(new Error('请输入正整数'))
            } else {
                callback()
            }
        }
    }, 0)
}

// 13、验证是否整数,非必填
export function isIntegerNotMust(rule, value, callback) {
    if (!value) {
        callback()
    }
    setTimeout(() => {
        if (!Number(value)) {
            callback(new Error('请输入正整数'))
        } else {
            const re = /^[0-9]*[1-9][0-9]*$/
            const rsCheck = re.test(value)
            if (!rsCheck) {
                callback(new Error('请输入正整数'))
            } else {
                callback()
            }
        }
    }, 1000)
}

// 14、 验证是否是[0-1]的小数
export function isDecimal(rule, value, callback) {
    if (!value) {
        return callback(new Error('输入不可以为空'))
    }
    setTimeout(() => {
        if (!Number(value)) {
            callback(new Error('请输入[0,1]之间的数字'))
        } else {
            if (value < 0 || value > 1) {
                callback(new Error('请输入[0,1]之间的数字'))
            } else {
                callback()
            }
        }
    }, 100)
}

// 15、 验证是否是[1-10]的小数,即不可以等于0
export function isBtnOneToTen(rule, value, callback) {
    if (typeof value === 'undefined') {
        return callback(new Error('输入不可以为空'))
    }
    setTimeout(() => {
        if (!Number(value)) {
            callback(new Error('请输入正整数，值为[1,10]'))
        } else {
            if (!(value === '1' || value === '2' || value === '3' || value === '4' || value === '5' || value === '6' || value === '7' || value === '8' || value === '9' || value === '10')) {
                callback(new Error('请输入正整数，值为[1,10]'))
            } else {
                callback()
            }
        }
    }, 100)
}

// 16、验证是否是[1-100]的小数,即不可以等于0

export function isBtnOneToHundredNotZero(rule, value, callback) {
    if (!value) {
        return callback(new Error('输入不可以为空'))
    }
    setTimeout(() => {
        if (!Number(value)) {
            callback(new Error('请输入整数，值为[1,100]'))
        } else {
            if (value < 1 || value > 100) {
                callback(new Error('请输入整数，值为[1,100]'))
            } else {
                callback()
            }
        }
    }, 100)
}

17、验证是否是[0 - 100]的小数

export function isBtnOneToHundred(rule, value, callback) {
    if (!value) {
        return callback(new Error('输入不可以为空'))
    }
    setTimeout(() => {
        if (!Number(value)) {
            callback(new Error('请输入整数，值为[1,100]'))
        } else {
            if (value < 1 || value > 100) {
                callback(new Error('请输入整数，值为[1,100]'))
            } else {
                callback()
            }
        }
    }, 100)
}

// 18、验证端口是否在[0,65535]之间

export function isPort(rule, value, callback) {
    if (!value) {
        return callback(new Error('输入不可以为空'))
    }
    setTimeout(() => {
        if (value === '' || typeof (value) === undefined) {
            callback(new Error('请输入端口值'))
        } else {
            const re = /^([0-9]|[1-9]\d|[1-9]\d{2}|[1-9]\d{3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$/
            const rsCheck = re.test(value)
            if (!rsCheck) {
                callback(new Error('请输入在[0-65535]之间的端口值'))
            } else {
                callback()
            }
        }
    }, 100)
}

// 19、验证端口是否在[0,65535]之间，非必填,isMust表示是否必填

export function isCheckPort(rule, value, callback) {
    if (!value) {
        callback()
    }
    setTimeout(() => {
        if (value === '' || typeof (value) === undefined) {
            // callback(new Error('请输入端口值'));
        } else {
            const re = /^([0-9]|[1-9]\d|[1-9]\d{2}|[1-9]\d{3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$/
            const rsCheck = re.test(value)
            if (!rsCheck) {
                callback(new Error('请输入在[0-65535]之间的端口值'))
            } else {
                callback()
            }
        }
    }, 100)
}

// 20、小写字母

export function validateLowerCase(val) {
    const reg = /^[a-z]+$/
    return reg.test(val)
}

// 21、两位小数验证

export const validateValidity = (rule, value, callback) => {
    if (!/(^[1-9]([0-9]+)?(\.[0-9]{1,2})?$)|(^(0){ 1 }$)|(^[0-9]\.[0-9]([0-9])?$)/.test(value)) {
        callback(new Error('最多两位小数！！！'))
    } else {
        callback()
    }
}

// 22、是否大写字母

export function validateUpperCase(val) {
    const reg = /^[A-Z]+$/
    return reg.test(val)
}

// 23、是否大小写字母

export function validateAlphabets(val) {
    const reg = /^[A-Za-z]+$/
    return reg.test(val)
}

// 24、密码校验

export function validatePsdReg(rule, value, callback) {
    if (!value) {
        return callback(new Error('密码不能为空'))
    }
    if (!/^(?![\d]+$)(?![a-zA-Z]+$)(?![^\da-zA-Z]+$)([^\u4e00-\u9fa5\s]){6,20}$/.test(value)) {
        callback(new Error('请输入6-20位英文字母、数字或者符号（除空格），且字母、数字和标点符号至少包含两种'))
    } else {
        callback()
    }
}

// 25、中文校验

export const validateContacts = (rule, value, callback) => {
    if (!value) {
        return callback(new Error('请输入中文'))
    }
    if (!/^[\u0391-\uFFE5A-Za-z]+$/.test(value)) {
        callback(new Error('不可输入特殊字符'))
    } else {
        callback()
    }
}

// 26、身份证校验

export const ID = (rule, value, callback) => {
    if (!value) {
        return callback(new Error('身份证不能为空'))
    }
    if (!/(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/.test(value)) {
        callback(new Error('请输入正确的二代身份证号码'))
    } else {
        callback()
    }
}

// 27、 账号校验

export const validateCode = (rule, value, callback) => {
    if (!value) {
        return callback(new Error('请输入账号'))
    }
    if (!/^(?![0-9]*$)(?![a-zA-Z]*$)[a-zA-Z0-9]{6,20}$/.test(value)) {
        callback(new Error('账号必须为6-20位字母和数字组合'))
    } else {
        callback()
    }
}

// 28 、纯数字校验

export const validateNumber = (rule, value, callback) => {
    const numberReg = /^\d+$|^\d+[.]?\d+$/
    if (value !== '') {
        if (!numberReg.test(value)) {
            callback(new Error('请输入数字'))
        } else {
            callback()
        }
    } else {
        callback(new Error('请输入值'))
    }
}

//29、最多一位小数

export const onePoint = (rule, value, callback) => {
    if (!/^[0-9]+([.]{1}[0-9]{1})?$/.test(value)) {
        callback(new Error('最多一位小数！！！'))
    } else {
        callback()
    }
}

const vcity = {
    11: '北京', 12: '天津', 13: '河北', 14: '山西', 15: '内蒙古',
    21: '辽宁', 22: '吉林', 23: '黑龙江', 31: '上海', 32: '江苏',
    33: '浙江', 34: '安徽', 35: '福建', 36: '江西', 37: '山东', 41: '河南',
    42: '湖北', 43: '湖南', 44: '广东', 45: '广西', 46: '海南', 50: '重庆',
    51: '四川', 52: '贵州', 53: '云南', 54: '西藏', 61: '陕西', 62: '甘肃',
    63: '青海', 64: '宁夏', 65: '新疆', 71: '台湾', 81: '香港', 82: '澳门', 91: '国外'
}

function checkCard(obj) {
    // 校验长度，类型
    if (isCardNo(obj) === false) { return false }
    // 检查省份
    if (checkProvince(obj) === false) { return false }
    // 校验生日
    if (checkBirthday(obj) === false) { return false }
    // 检验位的检测
    return true
    // return checkParity(obj) !== false
}

// 检查号码是否符合规范，包括长度，类型
function isCardNo(obj) {
    // 身份证号码为15位或者18位，15位时全为数字，18位前17位为数字，最后一位是校验位，可能为数字或字符X
    const reg = /(^\d{15}$)|(^\d{17}(\d|X)$)/
    return reg.test(obj) !== false
}

// 取身份证前两位,校验省份
function checkProvince(obj) {
    const province = obj.substr(0, 2)
    return vcity[province] !== undefined
}

// 检查生日是否正确
function checkBirthday(obj) {
    let birthday
    let day
    let month
    let year
    let arr_data
    const len = obj.length
    // 身份证15位时，次序为省（3位）市（3位）年（2位）月（2位）日（2位）校验位（3位），皆为数字
    if (len === 15) {
        const re_fifteen = /^(\d{6})(\d{2})(\d{2})(\d{2})(\d{3})$/
        arr_data = obj.match(re_fifteen)
        year = arr_data[2]
        month = arr_data[3]
        day = arr_data[4]
        birthday = new Date('19' + year + '/' + month + '/' + day)
        return verifyBirthday('19' + year, month, day, birthday)
    }
    // 身份证18位时，次序为省（3位）市（3位）年（4位）月（2位）日（2位）校验位（4位），校验位末尾可能为X
    if (len === 18) {
        const re_eighteen = /^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})([0-9]|X)$/
        arr_data = obj.match(re_eighteen)
        year = arr_data[2]
        month = arr_data[3]
        day = arr_data[4]
        birthday = new Date(year + '/' + month + '/' + day)
        return verifyBirthday(year, month, day, birthday)
    }
    return false
}

// 校验日期
function verifyBirthday(year, month, day, birthday) {
    const now = new Date()
    const now_year = now.getFullYear()
    if (month[0] === '0') { month = month[1] }
    if (day[0] === '0') { day = day[1] }
    // 年月日是否合理
    if ('' + birthday.getFullYear() === year && '' + (birthday.getMonth() + 1) === month && '' + birthday.getDate() === day) {
        // 判断年份的范围（3岁到100岁之间)
        const time = now_year - year
        return time >= 0 && time <= 130
    }
    return false
}

// 校验位的检测
// eslint-disable-next-line no-unused-vars
function checkParity(obj) {
    // 15位转18位
    obj = changeFivteenToEighteen(obj)
    const len = obj.length
    if (len === 18) {
        const arrInt = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2]
        const arrCh = ['1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2']
        let cardTemp = 0
        let i
        var valnum
        for (i = 0; i < 17; i++) {
            cardTemp += obj.substr(i, 1) * arrInt[i]
        }
        valnum = arrCh[cardTemp % 11]
        return valnum === obj.substr(16, 1)
    }
    return false
}

// 15位转18位身份证号
function changeFivteenToEighteen(obj) {
    if (obj.length === '15') {
        const arrInt = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2]
        const arrCh = ['1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2']
        let cardTemp = 0; let i
        obj = obj.substr(0, 6) + '19' + obj.substr(6, obj.length - 6)
        for (i = 0; i < 17; i++) {
            cardTemp += obj.substr(i, 1) * arrInt[i]
        }
        obj += arrCh[cardTemp % 11]
        return obj
    }
    return obj
}