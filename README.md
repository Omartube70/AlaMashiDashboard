# ![AlaMashi Logo](https://github.com/Omartube70/AlaMashiDashboard/blob/master/AlaMashiDashboard/wwwroot/favicon.png?raw=true)

# 🧭 AlaMashi Dashboard

لوحة التحكم الإدارية الخاصة بمشروع **عالماشي**، باستخدام تقنيات حديثة من مايكروسوفت (.NET & Blazor Server) مع هيكلية نظيفة (Clean Architecture) في الـ Backend.

---

## 🧱 مكونات النظام

🔹 **AlaMashi Dashboard (الواجهة الأمامية)**  
تم تطويرها بـ **Blazor Server** لتوفير تجربة مستخدم تفاعلية وسريعة.  
📂 [رابط المشروع على GitHub](https://github.com/Omartube70/AlaMashiDashboard)

🔹 **AlaMashi.API (الخادم الخلفي)**  
تم تطويره باستخدام **ASP.NET Core Web API**  
ويعتمد على **Entity Framework Core** ضمن **Clean Architecture** لضمان فصل الطبقات وسهولة التطوير المستقبلي.  
📂 [رابط الـ API على GitHub](https://github.com/Omartube70/AlaMashi.API)

---

## 🧩 نظرة عامة

نظام متكامل لإدارة متجر إلكتروني يحتوي على:
- إدارة الطلبات والمدفوعات
- إدارة المنتجات والعروض
- إدارة المستخدمين والعناوين
- تقارير وإحصائيات دقيقة (يومي / شهري / سنوي)
- مصادقة JWT آمنة
- دعم اللغة العربية بالكامل

---

## ⚙️ التقنيات المستخدمة

### 🖥️ الواجهة الأمامية (Frontend)
- **Blazor Server**  
- **MudBlazor** (مكتبة المكونات)
- **Chart.js** (للرسوم البيانية)
- **HTML5 / CSS3**
- **ProtectedLocalStorage** (لتخزين الجلسات محلياً)

### 🧠 الواجهة الخلفية (Backend)
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **AutoMapper**
- **FluentValidation**
- **JWT Authentication**
- **Clean Architecture**  
  > طبقات: Domain - Application - Infrastructure - API

---

## 🌟 المميزات الرئيسية

- 📊 **لوحة تحكم تفاعلية بالكامل**
- 🧾 **إدارة الطلبات والمدفوعات**
- 🛒 **إدارة المنتجات والعروض**
- 👥 **إدارة المستخدمين والصلاحيات**
- 💰 **تقارير الإيرادات والأداء**
- 🔒 **تسجيل دخول آمن JWT**
- 🌍 **واجهة عربية بالكامل**
- ⚡ **أداء سريع واستجابة فورية**

---

## 🖼️ لقطات من الواجهة

| الصفحة | الصورة |
|--------|---------|
| 🏠 لوحة التحكم الرئيسية | ![Dashboard](https://github.com/Omartube70/AlaMashiDashboard/blob/master/AlaMashiDashboard/Images/DashBord.jpg?raw=true) |
| 🧾 تفاصيل الطلب | ![Order Details](https://github.com/Omartube70/AlaMashiDashboard/blob/master/AlaMashiDashboard/Images/OrderDetails.jpg?raw=true) |
| 🛍️ تفاصيل المنتج | ![Product Details](https://github.com/Omartube70/AlaMashiDashboard/blob/master/AlaMashiDashboard/Images/AddProduct.jpg?raw=true) |
| 👤 تفاصيل المستخدم | ![User Details](https://github.com/Omartube70/AlaMashiDashboard/blob/master/AlaMashiDashboard/Images/UserDetails.jpg?raw=true) |
| 🎯 تفاصيل العرض | ![Offer Details](https://github.com/Omartube70/AlaMashiDashboard/blob/master/AlaMashiDashboard/Images/OfferDetails.jpg?raw=true) |
| ⚙️ الصفحة الشخصية | ![Profile](https://github.com/Omartube70/AlaMashiDashboard/blob/master/AlaMashiDashboard/Images/Profile.jpg?raw=true) |

---

## 📈 التقارير والإحصائيات

- **إجمالي الطلبات والإيرادات**
- **إيراد اليوم / الشهر / السنة**
- **أكثر المنتجات مبيعاً**
- **نسبة الطلبات حسب الحالة**
- **نمو المبيعات على مدار الوقت**

---

## 🔐 الأمان

- مصادقة **JWT Token**
- تشفير الجلسات باستخدام **ProtectedBrowserStorage**
- حماية من الطلبات الغير مصرح بها (Unauthorized Access)
- تحديث تلقائي للـ Token

---

## 🚀 الأداء

- تحميل البيانات تدريجياً (Lazy Loading)
- تحسين الاستعلامات في EF Core
- تقليل استهلاك الذاكرة
- تجربة استخدام سلسة حتى مع بيانات ضخمة

---

## 🧑‍💻 المطور

تم تطوير النظام بالكامل بواسطة:

**عمر – مبرمج عمر**  
💻 Backend & Frontend Developer  
⚙️ متخصص في:  
`C#`, `.NET`, `Blazor`, `Entity Framework`, `SQL Server`, `Clean Architecture`

📍 GitHub: [Omartube70](https://github.com/Omartube70)

---

## 🧱 هيكل المشروع (Architecture Overview)



AlaMashi.API
│
├── Domain
│ └── الكيانات والقواعد (Entities, Enums, Interfaces)
│
├── Application
│ └── الخدمات (Services) والمنطق (Business Rules)
│
├── Infrastructure
│ └── التعامل مع قاعدة البيانات وملفات الإعداد
│
└── API
└── Controllers + Endpoints


---

## 🏁 الحالة الحالية

- ✅ النسخة الأولى تعمل بكامل الوظائف الأساسية  
- 🔄 يتم حالياً تطوير:
  - نظام الإشعارات الفورية (Notifications)
  - تصدير التقارير (PDF / Excel)
  - تحسين واجهة المستخدم Dashboard

---

© 2025 - تم تطوير مشروع AlaMashi Dashboard & API بواسطة مبرمج عمر. جميع الحقوق محفوظة.
