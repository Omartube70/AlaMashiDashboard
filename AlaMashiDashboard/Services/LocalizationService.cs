using AlaMashiDashboard.Components.Pages.Mangement_Date;
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

                    // Orders
                    ["orders.title"] = "Orders Management",
                    ["orders.subtitle"] = "Track and manage all customer orders",
                    ["orders.id"] = "Order ID",
                    ["orders.customer_name"] = "Customer Name",
                    ["orders.phone"] = "Phone",
                    ["orders.delivery_date"] = "Delivery Date",
                    ["orders.delivery_time"] = "Delivery Time",
                    ["orders.status"] = "Status",
                    ["orders.total"] = "Total Amount",
                    ["orders.actions"] = "Actions",
                    ["orders.pending"] = "Pending",
                    ["orders.in_preparation"] = "In Preparation",
                    ["orders.out_for_delivery"] = "Out for Delivery",
                    ["orders.delivered"] = "Delivered",
                    ["orders.canceled"] = "Canceled",
                    ["orders.search_placeholder"] = "Search orders...",
                    ["orders.filter_status"] = "Filter by Status",
                    ["orders.showing"] = "Showing",
                    ["orders.of"] = "of",
                    ["orders.orders"] = "orders",
                    ["orders.rows_per_page"] = "Rows per page",
                    ["orders.empty"] = "No Orders Found",
                    ["orders.empty_msg"] = "Orders will appear here once customers start placing them.",
                    ["orders.update_status"] = "Update Status",
                    ["orders.details"] = "Order Details",
                    ["orders.customer_info"] = "Customer Information",
                    ["orders.delivery_window"] = "Delivery Window",
                    ["orders.created_at"] = "Created At",

                    // Payments
                    ["payments.title"] = "Payments Management",
                    ["payments.subtitle"] = "Track and manage all payment transactions",
                    ["payments.id"] = "Payment ID",
                    ["payments.order_id"] = "Order ID",
                    ["payments.transaction"] = "Transaction ID",
                    ["payments.amount"] = "Amount",
                    ["payments.method"] = "Payment Method",
                    ["payments.status"] = "Status",
                    ["payments.date"] = "Date",
                    ["payments.cash"] = "Cash",
                    ["payments.card"] = "Card",
                    ["payments.wallet"] = "Wallet",
                    ["payments.online"] = "Online",
                    ["payments.pending"] = "Pending",
                    ["payments.paid"] = "Paid",
                    ["payments.refunded"] = "Refunded",
                    ["payments.failed"] = "Failed",
                    ["payments.total_revenue"] = "Total Revenue",
                    ["payments.search_placeholder"] = "Search payments...",
                    ["payments.filter_status"] = "Filter by Status",
                    ["payments.rows_per_page"] = "Rows per page",
                    ["payments.showing"] = "Showing",
                    ["payments.of"] = "of",
                    ["payments.payments"] = "payments",
                    ["payments.empty"] = "No Payments Found",
                    ["payments.empty_msg"] = "Payments will appear here once customers complete their orders.",



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

                    // Dashboard  جديدة
                    ["dashboard.revenue_analysis"] = "Revenue Analysis",
                    ["dashboard.order_status"] = "Order Status",
                    ["dashboard.top_selling"] = "Top Selling Products",
                    ["dashboard.order_stats"] = "Order Statistics",
                    ["dashboard.no_sales"] = "No sales data available yet",
                    ["dashboard.failed_load"] = "Failed to Load Dashboard",
                    ["dashboard.failed_msg"] = "Unable to fetch dashboard data. Please try again.",

                    // Orders  للتقارير
                    ["orders.recent"] = "Recent Orders",
                    ["orders.revenue_today"] = "Today's Revenue",
                    ["orders.revenue_month"] = "Monthly Revenue",
                    ["orders.total_revenue"] = "Total Revenue",


                    // Orders - إضافات
                    ["orders.order_id"] = "Order ID",
                    ["orders.customer_name"] = "Customer Name",
                    ["orders.total_amount"] = "Total Amount",
                    ["orders.order_date"] = "Order Date",
                    ["orders.delivery_date"] = "Delivery Date",
                    ["orders.view_details"] = "View Details",
                    ["orders.all_orders"] = "All Orders",
                    ["orders.loading"] = "Loading orders...",
                    ["orders.no_orders"] = "No Orders Found",
                    ["orders.no_orders_msg"] = "Orders will appear here once customers start placing them.",
                    ["orders.retry"] = "Retry",
                    ["orders.filter_by_status"] = "Filter by Status",

                    ["payments.payment_id"] = "Payment ID",
                    ["payments.transaction_id"] = "Transaction ID",
                    ["payments.payment_method"] = "Payment Method",
                    ["payments.payment_date"] = "Payment Date",
                    ["payments.payment_status"] = "Payment Status",
                    ["payments.all_payments"] = "All Payments",
                    ["payments.credit_card"] = "Credit Card",
                    ["payments.debit_card"] = "Debit Card",
                    ["payments.loading"] = "Loading payments...",
                    ["payments.no_payments"] = "No Payments Found",
                    ["payments.no_payments_msg"] = "Payment transactions will appear here once orders are placed.",
                    ["payments.retry"] = "Retry",
                    ["payments.filter_by_status"] = "Filter by Status",
                    ["payments.completed"] = "Completed",
                    ["payments.canceled"] = "Canceled",

                    // Profile - إضافات
                    ["profile.active"] = "Active",
                    ["profile.updated"] = "Profile updated successfully",
                    ["profile.password_updated"] = "Password updated successfully",
                    ["profile.passwords_not_match"] = "Passwords do not match",
                    ["profile.no_changes"] = "No changes to save",
                    ["profile.cancel"] = "Cancel",

                   // Orders - إضافات
         ["orders.order_id"] = "Order ID",
                    ["orders.customer_name"] = "Customer Name",
                    ["orders.total_amount"] = "Total Amount",
                    ["orders.order_date"] = "Order Date",
                    ["orders.delivery_date"] = "Delivery Date",
                    ["orders.view_details"] = "View Details",
                    ["orders.all_orders"] = "All Orders",
                    ["orders.loading"] = "Loading orders...",
                    ["orders.no_orders"] = "No Orders Found",
                    ["orders.no_orders_msg"] = "Orders will appear here once customers start placing them.",
                    ["orders.retry"] = "Retry",
                    ["orders.filter_by_status"] = "Filter by Status",

                    // Payments - إضافات
                    ["payments.payment_id"] = "Payment ID",
                    ["payments.transaction_id"] = "Transaction ID",
                    ["payments.payment_method"] = "Payment Method",
                    ["payments.payment_date"] = "Payment Date",
                    ["payments.payment_status"] = "Payment Status",
                    ["payments.all_payments"] = "All Payments",
                    ["payments.credit_card"] = "Credit Card",
                    ["payments.debit_card"] = "Debit Card",
                    ["payments.loading"] = "Loading payments...",
                    ["payments.no_payments"] = "No Payments Found",
                    ["payments.no_payments_msg"] = "Payment transactions will appear here once orders are placed.",
                    ["payments.retry"] = "Retry",
                    ["payments.filter_by_status"] = "Filter by Status",
                    ["payments.completed"] = "Completed",
                    ["payments.canceled"] = "Canceled",

                    // Profile - إضافات
                    ["profile.active"] = "Active",
                    ["profile.updated"] = "Profile updated successfully",
                    ["profile.password_updated"] = "Password updated successfully",
                    ["profile.passwords_not_match"] = "Passwords do not match",
                    ["profile.no_changes"] = "No changes to save",
                    ["profile.cancel"] = "Cancel",

                    // Dialogs - Categories
                    ["dialog.add_category"] = "Add New Category",
                    ["dialog.edit_category"] = "Edit Category",
                    ["dialog.category_name"] = "Category Name",
                    ["dialog.category_name_placeholder"] = "e.g., Electronics",
                    ["dialog.icon_name"] = "Icon Name",
                    ["dialog.icon_helper"] = "Material Design icon name (e.g., Devices, LocalOffer)",
                    ["dialog.parent_category"] = "Parent Category (Optional)",
                    ["dialog.no_parent"] = "No Parent (Root Category)",
                    ["dialog.creating"] = "Creating...",
                    ["dialog.create"] = "Create Category",
                    ["dialog.updating"] = "Updating...",
                    ["dialog.update"] = "Update Category",

                    // Dialogs - Products
                    ["dialog.add_product"] = "Add New Product",
                    ["dialog.edit_product"] = "Edit Product",
                    ["dialog.product_name"] = "Product Name",
                    ["dialog.product_name_placeholder"] = "e.g., Wireless Headphones",
                    ["dialog.description"] = "Description",
                    ["dialog.description_placeholder"] = "Describe your product...",
                    ["dialog.price"] = "Price (EGP)",
                    ["dialog.quantity"] = "Quantity in Stock",
                    ["dialog.category"] = "Category",
                    ["dialog.select_category"] = "-- Select Category --",
                    ["dialog.image_url"] = "Image URL (Optional)",
                    ["dialog.image_url_placeholder"] = "https://example.com/image.jpg",
                    ["dialog.image_helper"] = "Provide a URL to the product image",

                    // Dialogs - Offers
                    ["dialog.add_offer"] = "Add New Offer",
                    ["dialog.edit_offer"] = "Edit Offer",
                    ["dialog.offer_title"] = "Offer Title",
                    ["dialog.offer_title_placeholder"] = "e.g., Summer Sale",
                    ["dialog.discount_percentage"] = "Discount Percentage (%)",
                    ["dialog.start_date"] = "Start Date",
                    ["dialog.end_date"] = "End Date",

                    // Dialogs - Orders
                    ["dialog.order_details"] = "Order Details",
                    ["dialog.order_info"] = "Order Information",
                    ["dialog.customer_info"] = "Customer Information",
                    ["dialog.delivery_info"] = "Delivery Information",
                    ["dialog.items"] = "Order Items",
                    ["dialog.payment_info"] = "Payment Information",
                    ["dialog.update_order_status"] = "Update Order Status",
                    ["dialog.current_status"] = "Current Status",
                    ["dialog.new_status"] = "New Status",
                    ["dialog.status_confirmation"] = "Are you sure you want to update the order status?",

                    // Validation Messages
                    ["validation.required"] = "This field is required",
                    ["validation.min_length"] = "Minimum length is {0} characters",
                    ["validation.max_length"] = "Maximum length is {0} characters",
                    ["validation.invalid_format"] = "Invalid format",
                    ["validation.min_value"] = "Minimum value is {0}",
                    ["validation.max_value"] = "Maximum value is {0}",


                    // Reports
                    ["reports.title"] = "Reports & Analytics",
                    ["reports.subtitle"] = "Visualize sales, performance, and customer insights",
                    ["reports.daily_income"] = "Daily Income",
                    ["reports.monthly_income"] = "Monthly Income",
                    ["reports.orders_chart"] = "Orders Overview",
                    ["reports.revenue_chart"] = "Revenue Trend",

                    // Errors
                    ["error.network"] = "Network Error",
                    ["error.unauthorized"] = "Unauthorized Access",
                    ["error.timeout"] = "Request Timed Out",
                    ["error.notfound"] = "Not Found",

                },
                ["ar"] = new Dictionary<string, string>
                {
                    // App
                    ["app.title"] = "عالماشي",
                    ["app.dashboard"] = "لوحة تحكم عالماشي",

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


                    // Orders
                    ["orders.title"] = "إدارة الطلبات",
                    ["orders.subtitle"] = "تتبع وإدارة جميع طلبات العملاء",
                    ["orders.id"] = "رقم الطلب",
                    ["orders.customer_name"] = "اسم العميل",
                    ["orders.phone"] = "الهاتف",
                    ["orders.delivery_date"] = "تاريخ التسليم",
                    ["orders.delivery_time"] = "وقت التسليم",
                    ["orders.status"] = "الحالة",
                    ["orders.total"] = "إجمالي المبلغ",
                    ["orders.actions"] = "الإجراءات",
                    ["orders.pending"] = "معلق",
                    ["orders.in_preparation"] = "قيد التحضير",
                    ["orders.out_for_delivery"] = "في التوصيل",
                    ["orders.delivered"] = "تم التوصيل",
                    ["orders.canceled"] = "ملغي",
                    ["orders.search_placeholder"] = "بحث في الطلبات...",
                    ["orders.filter_status"] = "تصفية حسب الحالة",
                    ["orders.showing"] = "عرض",
                    ["orders.of"] = "من",
                    ["orders.orders"] = "الطلبات",
                    ["orders.rows_per_page"] = "عدد الصفوف في الصفحة",
                    ["orders.empty"] = "لا توجد طلبات",
                    ["orders.empty_msg"] = "ستظهر الطلبات هنا بمجرد أن يبدأ العملاء في تقديمها.",
                    ["orders.update_status"] = "تحديث الحالة",
                    ["orders.details"] = "تفاصيل الطلب",
                    ["orders.customer_info"] = "معلومات العميل",
                    ["orders.delivery_window"] = "فترة التوصيل",
                    ["orders.created_at"] = "تم الإنشاء في",

                    // Payments
                    ["payments.title"] = "إدارة المدفوعات",
                    ["payments.subtitle"] = "تتبع وإدارة جميع معاملات الدفع",
                    ["payments.id"] = "رقم الدفع",
                    ["payments.order_id"] = "رقم الطلب",
                    ["payments.transaction"] = "رقم المعاملة",
                    ["payments.amount"] = "المبلغ",
                    ["payments.method"] = "طريقة الدفع",
                    ["payments.status"] = "الحالة",
                    ["payments.date"] = "التاريخ",
                    ["payments.cash"] = "نقدي",
                    ["payments.card"] = "بطاقة",
                    ["payments.wallet"] = "محفظة",
                    ["payments.online"] = "دفع إلكتروني",
                    ["payments.pending"] = "قيد الانتظار",
                    ["payments.paid"] = "مدفوع",
                    ["payments.refunded"] = "مسترد",
                    ["payments.failed"] = "فشل",
                    ["payments.total_revenue"] = "إجمالي الإيرادات",
                    ["payments.search_placeholder"] = "بحث في المدفوعات...",
                    ["payments.filter_status"] = "تصفية حسب الحالة",
                    ["payments.rows_per_page"] = "عدد الصفوف في الصفحة",
                    ["payments.showing"] = "عرض",
                    ["payments.of"] = "من",
                    ["payments.payments"] = "المدفوعات",
                    ["payments.empty"] = "لا توجد مدفوعات",
                    ["payments.empty_msg"] = "ستظهر معاملات الدفع هنا بمجرد إتمام الطلبات.",
                    ["payments.canceled"] = "ملغي",


                    ["orders.order_id"] = "رقم الطلب",
                    ["orders.customer_name"] = "اسم العميل",
                    ["orders.total_amount"] = "إجمالي المبلغ",
                    ["orders.order_date"] = "تاريخ الطلب",
                    ["orders.delivery_date"] = "تاريخ التسليم",
                    ["orders.view_details"] = "عرض التفاصيل",
                    ["orders.all_orders"] = "جميع الطلبات",
                    ["orders.loading"] = "جاري تحميل الطلبات...",
                    ["orders.no_orders"] = "لا توجد طلبات",
                    ["orders.no_orders_msg"] = "ستظهر الطلبات هنا بمجرد أن يبدأ العملاء في تقديمها.",
                    ["orders.retry"] = "إعادة المحاولة",
                    ["orders.filter_by_status"] = "تصفية حسب الحالة",

                    ["payments.payment_id"] = "رقم الدفع",
                    ["payments.transaction_id"] = "رقم المعاملة",
                    ["payments.payment_method"] = "طريقة الدفع",
                    ["payments.payment_date"] = "تاريخ الدفع",
                    ["payments.payment_status"] = "حالة الدفع",
                    ["payments.all_payments"] = "جميع المدفوعات",
                    ["payments.credit_card"] = "بطاقة ائتمان",
                    ["payments.debit_card"] = "بطاقة خصم",
                    ["payments.loading"] = "جاري تحميل المدفوعات...",
                    ["payments.no_payments"] = "لا توجد مدفوعات",
                    ["payments.no_payments_msg"] = "ستظهر معاملات الدفع هنا بمجرد إتمام الطلبات.",
                    ["payments.retry"] = "إعادة المحاولة",
                    ["payments.filter_by_status"] = "تصفية حسب الحالة",
                    ["payments.completed"] = "مكتمل",

                    // Profile - إضافات بالعربية
                    ["profile.active"] = "نشط",
                    ["profile.updated"] = "تم تحديث الملف الشخصي بنجاح",
                    ["profile.password_updated"] = "تم تحديث كلمة المرور بنجاح",
                    ["profile.passwords_not_match"] = "كلمات المرور غير متطابقة",
                    ["profile.no_changes"] = "لا توجد تغييرات لحفظها",
                    ["profile.cancel"] = "إلغاء",

                    ["profile.cancel"] = "إلغاء",

                    // Dialogs - Categories بالعربية
                    ["dialog.add_category"] = "إضافة تصنيف جديد",
                    ["dialog.edit_category"] = "تعديل التصنيف",
                    ["dialog.category_name"] = "اسم التصنيف",
                    ["dialog.category_name_placeholder"] = "مثال: إلكترونيات",
                    ["dialog.icon_name"] = "اسم الأيقونة",
                    ["dialog.icon_helper"] = "اسم أيقونة Material Design (مثال: Devices, LocalOffer)",
                    ["dialog.parent_category"] = "التصنيف الأب (اختياري)",
                    ["dialog.no_parent"] = "بدون أب (تصنيف رئيسي)",
                    ["dialog.creating"] = "جاري الإنشاء...",
                    ["dialog.create"] = "إنشاء التصنيف",
                    ["dialog.updating"] = "جاري التحديث...",
                    ["dialog.update"] = "تحديث التصنيف",

                    // Dialogs - Products بالعربية
                    ["dialog.add_product"] = "إضافة منتج جديد",
                    ["dialog.edit_product"] = "تعديل المنتج",
                    ["dialog.product_name"] = "اسم المنتج",
                    ["dialog.product_name_placeholder"] = "مثال: سماعات لاسلكية",
                    ["dialog.description"] = "الوصف",
                    ["dialog.description_placeholder"] = "اكتب وصف المنتج...",
                    ["dialog.price"] = "السعر (ج.م)",
                    ["dialog.quantity"] = "الكمية في المخزون",
                    ["dialog.category"] = "التصنيف",
                    ["dialog.select_category"] = "-- اختر التصنيف --",
                    ["dialog.image_url"] = "رابط الصورة (اختياري)",
                    ["dialog.image_url_placeholder"] = "https://example.com/image.jpg",
                    ["dialog.image_helper"] = "أدخل رابط صورة المنتج",

                    // Dialogs - Offers بالعربية
                    ["dialog.add_offer"] = "إضافة عرض جديد",
                    ["dialog.edit_offer"] = "تعديل العرض",
                    ["dialog.offer_title"] = "عنوان العرض",
                    ["dialog.offer_title_placeholder"] = "مثال: تخفيضات الصيف",
                    ["dialog.discount_percentage"] = "نسبة الخصم (%)",
                    ["dialog.start_date"] = "تاريخ البداية",
                    ["dialog.end_date"] = "تاريخ النهاية",

                    // Dialogs - Orders بالعربية
                    ["dialog.order_details"] = "تفاصيل الطلب",
                    ["dialog.order_info"] = "معلومات الطلب",
                    ["dialog.customer_info"] = "معلومات العميل",
                    ["dialog.delivery_info"] = "معلومات التوصيل",
                    ["dialog.items"] = "منتجات الطلب",
                    ["dialog.payment_info"] = "معلومات الدفع",
                    ["dialog.update_order_status"] = "تحديث حالة الطلب",
                    ["dialog.current_status"] = "الحالة الحالية",
                    ["dialog.new_status"] = "الحالة الجديدة",
                    ["dialog.status_confirmation"] = "هل أنت متأكد من تحديث حالة الطلب؟",

                    // Validation Messages بالعربية
                    ["validation.required"] = "هذا الحقل مطلوب",
                    ["validation.min_length"] = "الحد الأدنى للطول هو {0} أحرف",
                    ["validation.max_length"] = "الحد الأقصى للطول هو {0} أحرف",
                    ["validation.invalid_format"] = "تنسيق غير صالح",
                    ["validation.min_value"] = "الحد الأدنى للقيمة هو {0}",
                    ["validation.max_value"] = "الحد الأقصى للقيمة هو {0}",

                    // =====================================
                    // أضف هذه المفاتيح داخل ["ar"] في InitializeTranslations()

                    // Orders - إضافات بالعربية
                    ["orders.order_id"] = "رقم الطلب",
                    ["orders.customer_name"] = "اسم العميل",
                    ["orders.total_amount"] = "إجمالي المبلغ",
                    ["orders.order_date"] = "تاريخ الطلب",
                    ["orders.delivery_date"] = "تاريخ التسليم",
                    ["orders.view_details"] = "عرض التفاصيل",
                    ["orders.all_orders"] = "جميع الطلبات",
                    ["orders.loading"] = "جاري تحميل الطلبات...",
                    ["orders.no_orders"] = "لا توجد طلبات",
                    ["orders.no_orders_msg"] = "ستظهر الطلبات هنا بمجرد أن يبدأ العملاء في تقديمها.",
                    ["orders.retry"] = "إعادة المحاولة",
                    ["orders.filter_by_status"] = "تصفية حسب الحالة",

                    // Payments - إضافات بالعربية
                    ["payments.payment_id"] = "رقم الدفع",
                    ["payments.transaction_id"] = "رقم المعاملة",
                    ["payments.payment_method"] = "طريقة الدفع",
                    ["payments.payment_date"] = "تاريخ الدفع",
                    ["payments.payment_status"] = "حالة الدفع",
                    ["payments.all_payments"] = "جميع المدفوعات",
                    ["payments.credit_card"] = "بطاقة ائتمان",
                    ["payments.debit_card"] = "بطاقة خصم",
                    ["payments.loading"] = "جاري تحميل المدفوعات...",
                    ["payments.no_payments"] = "لا توجد مدفوعات",
                    ["payments.no_payments_msg"] = "ستظهر معاملات الدفع هنا بمجرد إتمام الطلبات.",
                    ["payments.retry"] = "إعادة المحاولة",
                    ["payments.filter_by_status"] = "تصفية حسب الحالة",
                    ["payments.completed"] = "مكتمل",
                    ["payments.canceled"] = "ملغي",

                    // Profile - إضافات بالعربية
                    ["profile.active"] = "نشط",
                    ["profile.updated"] = "تم تحديث الملف الشخصي بنجاح",
                    ["profile.password_updated"] = "تم تحديث كلمة المرور بنجاح",
                    ["profile.passwords_not_match"] = "كلمات المرور غير متطابقة",
                    ["profile.no_changes"] = "لا توجد تغييرات لحفظها",
                    ["profile.cancel"] = "إلغاء",
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

                    ["dashboard.revenue_analysis"] = "تحليل الإيرادات",
                    ["dashboard.order_status"] = "حالة الطلبات",
                    ["dashboard.top_selling"] = "المنتجات الأكثر مبيعاً",
                    ["dashboard.order_stats"] = "إحصائيات الطلبات",
                    ["dashboard.no_sales"] = "لا توجد بيانات مبيعات حتى الآن",
                    ["dashboard.failed_load"] = "فشل في تحميل لوحة التحكم",
                    ["dashboard.failed_msg"] = "تعذر تحميل بيانات لوحة التحكم. يرجى المحاولة مرة أخرى.",

                    ["orders.recent"] = "الطلبات الأخيرة",
                    ["orders.revenue_today"] = "إيرادات اليوم",
                    ["orders.revenue_month"] = "إيرادات الشهر",
                    ["orders.total_revenue"] = "إجمالي الإيرادات",

                    ["reports.title"] = "التقارير والتحليلات",
                    ["reports.subtitle"] = "عرض مرئي للمبيعات والأداء وسلوك العملاء",
                    ["reports.daily_income"] = "الدخل اليومي",
                    ["reports.monthly_income"] = "الدخل الشهري",
                    ["reports.orders_chart"] = "مخطط الطلبات",
                    ["reports.revenue_chart"] = "مخطط الإيرادات",

                    ["error.network"] = "خطأ في الاتصال بالشبكة",
                    ["error.unauthorized"] = "دخول غير مصرح به",
                    ["error.timeout"] = "انتهت مهلة الطلب",
                    ["error.notfound"] = "العنصر غير موجود",

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