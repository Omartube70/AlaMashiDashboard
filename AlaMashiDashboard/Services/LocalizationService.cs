using Microsoft.JSInterop;
using System.Text.Json;

namespace AdminDashboard.Services
{
    public class LocalizationService
    {
        private readonly IJSRuntime _js;
        private Dictionary<string, Dictionary<string, string>> _translations = new();
        private string _currentLanguage = "en";

        public event Func<Task>? OnLanguageChanged;

        public LocalizationService(IJSRuntime js)
        {
            _js = js;
            InitializeTranslations();
        }

        private void InitializeTranslations()
        {
            _translations = new Dictionary<string, Dictionary<string, string>>
            {
                ["en"] = new Dictionary<string, string>
                {
                    // App
                    ["app.title"] = "AlaMashi",
                    ["app.dashboard"] = "AlaMashi Dashboard",

                    // Menu
                    ["menu.dashboard"] = "Dashboard",
                    ["menu.users"] = "Users",
                    ["menu.categories"] = "Categories",
                    ["menu.products"] = "Products",
                    ["menu.offers"] = "Offers",
                    ["menu.addresses"] = "Addresses",
                    ["menu.orders"] = "Orders",
                    ["menu.payments"] = "Payments",
                    ["menu.profile"] = "Profile",

                    // Dashboard
                    ["dashboard.title"] = "Dashboard Overview",
                    ["dashboard.welcome"] = "Welcome",
                    ["dashboard.welcome_msg"] = "to your dashboard",
                    ["dashboard.total_orders"] = "Total Orders",
                    ["dashboard.today_revenue"] = "Today's Revenue",
                    ["dashboard.month_revenue"] = "Monthly Revenue",
                    ["dashboard.total_users"] = "Total Users",
                    ["dashboard.total_products"] = "Total Products",
                    ["dashboard.categories"] = "Categories",
                    ["dashboard.active_offers"] = "Active Offers",
                    ["dashboard.pending_orders"] = "Pending Orders",

                    // User
                    ["user.settings"] = "Settings",
                    ["user.logout"] = "Logout",

                    // Theme & Language
                    ["theme.toggle"] = "Toggle Dark/Light Mode",
                    ["lang.toggle"] = "Switch Language",

                    // Common
                    ["common.loading"] = "Loading...",
                    ["common.save"] = "Save",
                    ["common.cancel"] = "Cancel",
                    ["common.delete"] = "Delete",
                    ["common.edit"] = "Edit",
                    ["common.add"] = "Add",
                    ["common.refresh"] = "Refresh",
                    ["common.export"] = "Export CSV",
                    ["common.search"] = "Search...",
                    ["common.filter"] = "Filter",
                    ["common.all"] = "All",
                    ["common.active"] = "Active",
                    ["common.pending"] = "Pending",
                    ["common.completed"] = "Completed",
                    ["common.failed"] = "Failed",
                    ["common.canceled"] = "Canceled",
                    ["common.actions"] = "Actions",
                    ["common.view"] = "View Details",
                    ["common.close"] = "Close",
                    ["common.retry"] = "Retry",
                    ["common.of"] = "of",
                    ["common.items"] = "items",
                    ["common.showing"] = "Showing",
                    ["common.failed"] = "Failed",
                    ["common.success"] = "Success",
                    ["common.error"] = "Error",
                    ["common.warning"] = "Warning",
                    ["common.info"] = "Info",

                    // Units
                    ["units.egp"] = "EGP",
                    ["units.pieces"] = "pieces",

                    // Categories  
                    ["categories.id"] = "ID",

                    // Login
                    ["login.title"] = "Welcome Back",
                    ["login.subtitle"] = "Sign in to access the admin dashboard",
                    ["login.email"] = "Email Address",
                    ["login.password"] = "Password",
                    ["login.signin"] = "Sign In",
                    ["login.signing_in"] = "Signing In...",
                    ["login.forgot_password"] = "Forgot your password?",
                    ["login.footer"] = "© 2025 Ala Mashi Dashboard. All rights reserved.",

                    // Forgot Password
                    ["forgot.title"] = "Forgot Password?",
                    ["forgot.subtitle"] = "No worries! Enter your email and we'll send you reset instructions",
                    ["forgot.send"] = "Send Reset Link",
                    ["forgot.sending"] = "Sending...",
                    ["forgot.back"] = "Back to Login",
                    ["forgot.info"] = "Check your spam folder if you don't receive the email within a few minutes",

                    // Reset Password
                    ["reset.title"] = "Reset Your Password",
                    ["reset.subtitle"] = "Enter the OTP sent to your email and create a new password",
                    ["reset.otp"] = "OTP Code",
                    ["reset.otp_helper"] = "Enter the 6-digit code sent to your email",
                    ["reset.new_password"] = "New Password",
                    ["reset.confirm_password"] = "Confirm Password",
                    ["reset.requirements"] = "Password Requirements:",
                    ["reset.req_length"] = "At least 8 characters",
                    ["reset.req_number"] = "At least one number",
                    ["reset.req_special"] = "At least one special character (!@#$%^&*)",
                    ["reset.submit"] = "Reset Password",
                    ["reset.resetting"] = "Resetting...",

                    // Users
                    ["users.title"] = "User Management",
                    ["users.subtitle"] = "Manage and view all registered users in the system",
                    ["users.id"] = "ID",
                    ["users.username"] = "Username",
                    ["users.email"] = "Email",
                    ["users.phone"] = "Phone",
                    ["users.role"] = "Role",
                    ["users.admin"] = "Admin",
                    ["users.user"] = "User",
                    ["users.promote"] = "Promote to Admin",
                    ["users.total"] = "Total Users",
                    ["users.admins"] = "Admins",
                    ["users.regular"] = "Regular Users",
                    ["users.empty"] = "No Users Found",
                    ["users.empty_msg"] = "Start by adding your first user to the system.",

                    // Categories
                    ["categories.title"] = "Categories Management",
                    ["categories.subtitle"] = "Organize and manage product categories",
                    ["categories.add"] = "Add Category",
                    ["categories.name"] = "Category Name",
                    ["categories.icon"] = "Icon",
                    ["categories.parent"] = "Parent Category",
                    ["categories.root"] = "Root",
                    ["categories.total"] = "Total Categories",
                    ["categories.showing"] = "Showing",
                    ["categories.root_count"] = "Root Categories",
                    ["categories.empty"] = "No Categories Found",
                    ["categories.empty_msg"] = "Start by creating your first category to organize your products.",

                    // Products
                    ["products.title"] = "Product Management",
                    ["products.subtitle"] = "Add, edit, and manage all available products in the system",
                    ["products.add"] = "Add Product",
                    ["products.id"] = "ID",
                    ["products.name"] = "Product",
                    ["products.price"] = "Price",
                    ["products.stock"] = "Stock",
                    ["products.category"] = "Category",
                    ["products.total"] = "Total Products",
                    ["products.in_stock"] = "In Stock",
                    ["products.low_stock"] = "Low Stock",
                    ["products.out_stock"] = "Out of Stock",
                    ["products.empty"] = "No Products Available",
                    ["products.empty_msg"] = "Start by adding your first product to the inventory.",

                    // Offers
                    ["offers.title"] = "Offers Management",
                    ["offers.subtitle"] = "Create and control discounts and promotional offers",
                    ["offers.add"] = "Add Offer",
                    ["offers.id"] = "ID",
                    ["offers.title_col"] = "Title",
                    ["offers.description"] = "Description",
                    ["offers.discount"] = "Discount",
                    ["offers.period"] = "Period",
                    ["offers.status"] = "Status",
                    ["offers.upcoming"] = "Upcoming",
                    ["offers.expired"] = "Expired",
                    ["offers.empty"] = "No Offers Available",
                    ["offers.empty_msg"] = "Create your first promotional offer to attract more customers.",
                    ["offers.create_first"] = "Create First Offer",

                    // Addresses
                    ["addresses.title"] = "Addresses Management",
                    ["addresses.subtitle"] = "Manage all delivery addresses in the system",
                    ["addresses.id"] = "ID",
                    ["addresses.user"] = "User",
                    ["addresses.city"] = "City",
                    ["addresses.street"] = "Street",
                    ["addresses.details"] = "Details",
                    ["addresses.type"] = "Type",
                    ["addresses.home"] = "Home",
                    ["addresses.work"] = "Work",
                    ["addresses.other"] = "Other",
                    ["addresses.map"] = "View on Map",
                    ["addresses.empty"] = "No Addresses Found",
                    ["addresses.empty_msg"] = "Delivery addresses will appear here once users add them.",

                    // Orders
                    ["orders.title"] = "Orders Management",
                    ["orders.subtitle"] = "Track and manage all customer orders",
                    ["orders.id"] = "Order ID",
                    ["orders.customer"] = "Customer",
                    ["orders.total"] = "Total",
                    ["orders.date"] = "Order Date",
                    ["orders.delivery"] = "Delivery Date",
                    ["orders.status"] = "Status",
                    ["orders.in_preparation"] = "In Preparation",
                    ["orders.out_delivery"] = "Out for Delivery",
                    ["orders.delivered"] = "Delivered",
                    ["orders.update_status"] = "Update Status",
                    ["orders.empty"] = "No Orders Found",
                    ["orders.empty_msg"] = "Orders will appear here once customers start placing them.",

                    // Payments
                    ["payments.title"] = "Payments Management",
                    ["payments.subtitle"] = "Track and manage all payment transactions",
                    ["payments.id"] = "Payment ID",
                    ["payments.transaction"] = "Transaction ID",
                    ["payments.amount"] = "Amount",
                    ["payments.method"] = "Payment Method",
                    ["payments.date"] = "Date",
                    ["payments.status"] = "Status",
                    ["payments.cash"] = "Cash",
                    ["payments.card"] = "Card",
                    ["payments.wallet"] = "Wallet",
                    ["payments.total_revenue"] = "Total Revenue",
                    ["payments.empty"] = "No Payments Found",
                    ["payments.empty_msg"] = "Payment transactions will appear here once orders are placed and paid.",

                    // Profile
                    ["profile.title"] = "Profile Settings",
                    ["profile.subtitle"] = "Manage your account information and preferences",
                    ["profile.loading"] = "Loading profile...",
                    ["profile.personal"] = "Personal Information",
                    ["profile.username"] = "Username",
                    ["profile.username_note"] = "Username cannot be changed",
                    ["profile.email"] = "Email",
                    ["profile.phone"] = "Phone",
                    ["profile.role"] = "Role",
                    ["profile.account"] = "Account Status",
                    ["profile.status"] = "Status",
                    ["profile.member_since"] = "Member Since",
                    ["profile.change_password"] = "Change Password",
                    ["profile.current_password"] = "Current Password",
                    ["profile.new_password"] = "New Password",
                    ["profile.confirm_password"] = "Confirm Password",
                    ["profile.password_helper"] = "Min 8 characters with number and special char",
                    ["profile.save"] = "Save Changes",
                    ["profile.error"] = "Failed to Load Profile",
                    ["profile.error_msg"] = "Unable to fetch your profile. Please try again.",
                },
                ["ar"] = new Dictionary<string, string>
                {
                    // App
                    ["app.title"] = "عالماشي",
                    ["app.dashboard"] = "لوحة التحكم",

                    // Menu
                    ["menu.dashboard"] = "لوحة التحكم",
                    ["menu.users"] = "المستخدمين",
                    ["menu.categories"] = "التصنيفات",
                    ["menu.products"] = "المنتجات",
                    ["menu.offers"] = "العروض",
                    ["menu.addresses"] = "العناوين",
                    ["menu.orders"] = "الطلبات",
                    ["menu.payments"] = "المدفوعات",
                    ["menu.profile"] = "الملف الشخصي",

                    // Dashboard
                    ["dashboard.title"] = "نظرة عامة على لوحة التحكم",
                    ["dashboard.welcome"] = "مرحباً",
                    ["dashboard.welcome_msg"] = "في لوحة التحكم الخاصة بك",
                    ["dashboard.total_orders"] = "إجمالي الطلبات",
                    ["dashboard.today_revenue"] = "إيرادات اليوم",
                    ["dashboard.month_revenue"] = "الإيرادات الشهرية",
                    ["dashboard.total_users"] = "إجمالي المستخدمين",
                    ["dashboard.total_products"] = "إجمالي المنتجات",
                    ["dashboard.categories"] = "التصنيفات",
                    ["dashboard.active_offers"] = "العروض النشطة",
                    ["dashboard.pending_orders"] = "الطلبات المعلقة",

                    // User
                    ["user.settings"] = "الإعدادات",
                    ["user.logout"] = "تسجيل الخروج",

                    // Theme & Language
                    ["theme.toggle"] = "تبديل الوضع الداكن/الفاتح",
                    ["lang.toggle"] = "تبديل اللغة",

                    // Common
                    ["common.loading"] = "جاري التحميل...",
                    ["common.save"] = "حفظ",
                    ["common.cancel"] = "إلغاء",
                    ["common.delete"] = "حذف",
                    ["common.edit"] = "تعديل",
                    ["common.add"] = "إضافة",
                    ["common.refresh"] = "تحديث",
                    ["common.export"] = "تصدير CSV",
                    ["common.search"] = "بحث...",
                    ["common.filter"] = "تصفية",
                    ["common.all"] = "الكل",
                    ["common.active"] = "نشط",
                    ["common.pending"] = "معلق",
                    ["common.completed"] = "مكتمل",
                    ["common.failed"] = "فشل",
                    ["common.canceled"] = "ملغي",
                    ["common.actions"] = "الإجراءات",
                    ["common.view"] = "عرض التفاصيل",
                    ["common.close"] = "إغلاق",
                    ["common.retry"] = "إعادة المحاولة",
                    ["common.of"] = "من",
                    ["common.items"] = "عناصر",
                    ["common.showing"] = "عرض",
                    ["common.failed"] = "فشل",
                    ["common.success"] = "نجح",
                    ["common.error"] = "خطأ",
                    ["common.warning"] = "تحذير",
                    ["common.info"] = "معلومات",

                    // Units
                    ["units.egp"] = "ج.م",
                    ["units.pieces"] = "قطعة",

                    // Categories
                    ["categories.id"] = "الرقم",

                    // Login
                    ["login.title"] = "مرحباً بعودتك",
                    ["login.subtitle"] = "قم بتسجيل الدخول للوصول إلى لوحة التحكم",
                    ["login.email"] = "البريد الإلكتروني",
                    ["login.password"] = "كلمة المرور",
                    ["login.signin"] = "تسجيل الدخول",
                    ["login.signing_in"] = "جاري تسجيل الدخول...",
                    ["login.forgot_password"] = "هل نسيت كلمة المرور؟",
                    ["login.footer"] = "© 2025 لوحة تحكم على ماشي. جميع الحقوق محفوظة.",

                    // Forgot Password
                    ["forgot.title"] = "نسيت كلمة المرور؟",
                    ["forgot.subtitle"] = "لا تقلق! أدخل بريدك الإلكتروني وسنرسل لك تعليمات إعادة التعيين",
                    ["forgot.send"] = "إرسال رابط إعادة التعيين",
                    ["forgot.sending"] = "جاري الإرسال...",
                    ["forgot.back"] = "العودة لتسجيل الدخول",
                    ["forgot.info"] = "تحقق من مجلد الرسائل غير المرغوب فيها إذا لم تستلم البريد خلال بضع دقائق",

                    // Reset Password
                    ["reset.title"] = "إعادة تعيين كلمة المرور",
                    ["reset.subtitle"] = "أدخل رمز OTP المرسل إلى بريدك الإلكتروني وأنشئ كلمة مرور جديدة",
                    ["reset.otp"] = "رمز OTP",
                    ["reset.otp_helper"] = "أدخل الرمز المكون من 6 أرقام المرسل إلى بريدك",
                    ["reset.new_password"] = "كلمة المرور الجديدة",
                    ["reset.confirm_password"] = "تأكيد كلمة المرور",
                    ["reset.requirements"] = "متطلبات كلمة المرور:",
                    ["reset.req_length"] = "8 أحرف على الأقل",
                    ["reset.req_number"] = "رقم واحد على الأقل",
                    ["reset.req_special"] = "حرف خاص واحد على الأقل (!@#$%^&*)",
                    ["reset.submit"] = "إعادة تعيين كلمة المرور",
                    ["reset.resetting"] = "جاري إعادة التعيين...",

                    // Users
                    ["users.title"] = "إدارة المستخدمين",
                    ["users.subtitle"] = "إدارة وعرض جميع المستخدمين المسجلين في النظام",
                    ["users.id"] = "الرقم",
                    ["users.username"] = "اسم المستخدم",
                    ["users.email"] = "البريد الإلكتروني",
                    ["users.phone"] = "الهاتف",
                    ["users.role"] = "الدور",
                    ["users.admin"] = "مدير",
                    ["users.user"] = "مستخدم",
                    ["users.promote"] = "ترقية إلى مدير",
                    ["users.total"] = "إجمالي المستخدمين",
                    ["users.admins"] = "المدراء",
                    ["users.regular"] = "المستخدمون العاديون",
                    ["users.empty"] = "لا يوجد مستخدمون",
                    ["users.empty_msg"] = "ابدأ بإضافة أول مستخدم إلى النظام.",

                    // Categories
                    ["categories.title"] = "إدارة التصنيفات",
                    ["categories.subtitle"] = "تنظيم وإدارة تصنيفات المنتجات",
                    ["categories.add"] = "إضافة تصنيف",
                    ["categories.name"] = "اسم التصنيف",
                    ["categories.icon"] = "الأيقونة",
                    ["categories.parent"] = "التصنيف الأب",
                    ["categories.root"] = "رئيسي",
                    ["categories.total"] = "إجمالي التصنيفات",
                    ["categories.showing"] = "عرض",
                    ["categories.root_count"] = "التصنيفات الرئيسية",
                    ["categories.empty"] = "لا توجد تصنيفات",
                    ["categories.empty_msg"] = "ابدأ بإنشاء أول تصنيف لتنظيم منتجاتك.",

                    // Products
                    ["products.title"] = "إدارة المنتجات",
                    ["products.subtitle"] = "إضافة وتعديل وإدارة جميع المنتجات المتاحة في النظام",
                    ["products.add"] = "إضافة منتج",
                    ["products.id"] = "الرقم",
                    ["products.name"] = "المنتج",
                    ["products.price"] = "السعر",
                    ["products.stock"] = "المخزون",
                    ["products.category"] = "التصنيف",
                    ["products.total"] = "إجمالي المنتجات",
                    ["products.in_stock"] = "متوفر",
                    ["products.low_stock"] = "مخزون منخفض",
                    ["products.out_stock"] = "غير متوفر",
                    ["products.empty"] = "لا توجد منتجات متاحة",
                    ["products.empty_msg"] = "ابدأ بإضافة أول منتج إلى المخزون.",

                    // Offers
                    ["offers.title"] = "إدارة العروض",
                    ["offers.subtitle"] = "إنشاء والتحكم في الخصومات والعروض الترويجية",
                    ["offers.add"] = "إضافة عرض",
                    ["offers.id"] = "الرقم",
                    ["offers.title_col"] = "العنوان",
                    ["offers.description"] = "الوصف",
                    ["offers.discount"] = "الخصم",
                    ["offers.period"] = "الفترة",
                    ["offers.status"] = "الحالة",
                    ["offers.upcoming"] = "قادم",
                    ["offers.expired"] = "منتهي",
                    ["offers.empty"] = "لا توجد عروض متاحة",
                    ["offers.empty_msg"] = "أنشئ أول عرض ترويجي لجذب المزيد من العملاء.",
                    ["offers.create_first"] = "إنشاء أول عرض",

                    // Addresses
                    ["addresses.title"] = "إدارة العناوين",
                    ["addresses.subtitle"] = "إدارة جميع عناوين التسليم في النظام",
                    ["addresses.id"] = "الرقم",
                    ["addresses.user"] = "المستخدم",
                    ["addresses.city"] = "المدينة",
                    ["addresses.street"] = "الشارع",
                    ["addresses.details"] = "التفاصيل",
                    ["addresses.type"] = "النوع",
                    ["addresses.home"] = "المنزل",
                    ["addresses.work"] = "العمل",
                    ["addresses.other"] = "أخرى",
                    ["addresses.map"] = "عرض على الخريطة",
                    ["addresses.empty"] = "لا توجد عناوين",
                    ["addresses.empty_msg"] = "ستظهر عناوين التسليم هنا بمجرد أن يضيفها المستخدمون.",

                    // Orders
                    ["orders.title"] = "إدارة الطلبات",
                    ["orders.subtitle"] = "تتبع وإدارة جميع طلبات العملاء",
                    ["orders.id"] = "رقم الطلب",
                    ["orders.customer"] = "العميل",
                    ["orders.total"] = "الإجمالي",
                    ["orders.date"] = "تاريخ الطلب",
                    ["orders.delivery"] = "تاريخ التسليم",
                    ["orders.status"] = "الحالة",
                    ["orders.in_preparation"] = "قيد التحضير",
                    ["orders.out_delivery"] = "في التوصيل",
                    ["orders.delivered"] = "تم التوصيل",
                    ["orders.update_status"] = "تحديث الحالة",
                    ["orders.empty"] = "لا توجد طلبات",
                    ["orders.empty_msg"] = "ستظهر الطلبات هنا بمجرد أن يبدأ العملاء في تقديمها.",

                    // Payments
                    ["payments.title"] = "إدارة المدفوعات",
                    ["payments.subtitle"] = "تتبع وإدارة جميع معاملات الدفع",
                    ["payments.id"] = "رقم الدفع",
                    ["payments.transaction"] = "رقم المعاملة",
                    ["payments.amount"] = "المبلغ",
                    ["payments.method"] = "طريقة الدفع",
                    ["payments.date"] = "التاريخ",
                    ["payments.status"] = "الحالة",
                    ["payments.cash"] = "نقدي",
                    ["payments.card"] = "بطاقة",
                    ["payments.wallet"] = "محفظة",
                    ["payments.total_revenue"] = "إجمالي الإيرادات",
                    ["payments.empty"] = "لا توجد مدفوعات",
                    ["payments.empty_msg"] = "ستظهر معاملات الدفع هنا بمجرد تقديم الطلبات ودفعها.",

                    // Profile
                    ["profile.title"] = "إعدادات الملف الشخصي",
                    ["profile.subtitle"] = "إدارة معلومات حسابك وتفضيلاتك",
                    ["profile.loading"] = "جاري تحميل الملف الشخصي...",
                    ["profile.personal"] = "المعلومات الشخصية",
                    ["profile.username"] = "اسم المستخدم",
                    ["profile.username_note"] = "لا يمكن تغيير اسم المستخدم",
                    ["profile.email"] = "البريد الإلكتروني",
                    ["profile.phone"] = "الهاتف",
                    ["profile.role"] = "الدور",
                    ["profile.account"] = "حالة الحساب",
                    ["profile.status"] = "الحالة",
                    ["profile.member_since"] = "عضو منذ",
                    ["profile.change_password"] = "تغيير كلمة المرور",
                    ["profile.current_password"] = "كلمة المرور الحالية",
                    ["profile.new_password"] = "كلمة المرور الجديدة",
                    ["profile.confirm_password"] = "تأكيد كلمة المرور",
                    ["profile.password_helper"] = "8 أحرف على الأقل مع رقم وحرف خاص",
                    ["profile.save"] = "حفظ التغييرات",
                    ["profile.error"] = "فشل تحميل الملف الشخصي",
                    ["profile.error_msg"] = "تعذر جلب ملفك الشخصي. يرجى المحاولة مرة أخرى.",
                }
            };
        }

        public async Task InitializeAsync()
        {
            try
            {
                var savedLang = await _js.InvokeAsync<string>("localStorage.getItem", "app_language");
                if (!string.IsNullOrEmpty(savedLang) && _translations.ContainsKey(savedLang))
                {
                    _currentLanguage = savedLang;
                }
            }
            catch
            {
                _currentLanguage = "en";
            }
        }

        public string CurrentLanguage => _currentLanguage;
        public bool IsArabic => _currentLanguage == "ar";
        public string Direction => IsArabic ? "rtl" : "ltr";

        public string Get(string key)
        {
            if (_translations.TryGetValue(_currentLanguage, out var langDict))
            {
                if (langDict.TryGetValue(key, out var value))
                {
                    return value;
                }
            }
            return key;
        }

        public async Task ToggleLanguageAsync()
        {
            _currentLanguage = _currentLanguage == "en" ? "ar" : "en";

            try
            {
                await _js.InvokeVoidAsync("localStorage.setItem", "app_language", _currentLanguage);
                await _js.InvokeVoidAsync("eval", $"document.documentElement.setAttribute('dir', '{Direction}')");
                await _js.InvokeVoidAsync("eval", $"document.documentElement.setAttribute('lang', '{_currentLanguage}')");
            }
            catch { }

            OnLanguageChanged?.Invoke();
        }

        public async Task SetLanguageAsync(string language)
        {
            if (_translations.ContainsKey(language))
            {
                _currentLanguage = language;

                try
                {
                    await _js.InvokeVoidAsync("localStorage.setItem", "app_language", _currentLanguage);
                    await _js.InvokeVoidAsync("eval", $"document.documentElement.setAttribute('dir', '{Direction}')");
                    await _js.InvokeVoidAsync("eval", $"document.documentElement.setAttribute('lang', '{_currentLanguage}')");
                }
                catch { }

                OnLanguageChanged?.Invoke();
            }
        }
    }
}