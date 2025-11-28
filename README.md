<img width="948" height="606" alt="image" src="https://github.com/user-attachments/assets/75caf656-48b2-4715-86c8-1ae058cd11a8" />

UI:
<img width="791" height="544" alt="image" src="https://github.com/user-attachments/assets/505f0ff8-e425-4f5a-8c6e-ba08c33ff14b" />

Include:
1. Implement a desktop App as front end by WPF MVVM.
2. The data can serviced by cache and backend service. Switch the feature flag on InventoryManagementApp->App.config->UseSqlDb, true use API, false use cache.
3. API Url config by  InventoryManagementApp->App.config->ServiceUrl, default is http://localhost:5016
4. Focus on desktop App,so backend API is not polished.
5. Use EventAggregator to decouple the viewmodels communication.
6. Design multiple layers architecture.
7. Build a shared project between front end and back end.
8. Dependency Injection on App start.

Not Include:
1. Logs and monitor.
2. Authentication and Authorization.
3. Cache layer to improve the performance.
