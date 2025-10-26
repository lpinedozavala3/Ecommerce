// core/models/Paged.ts
export interface PagedResponse<T> {
  status: number;              // Status
  message: string;             // Message
  errors?: string[];           // Errors
  data: T[];                   // Data (lista)
  pageNumber: number;          // PageNumber (1-based)
  pageSize: number;            // PageSize
  firstPage: string;           // FirstPage (URL)
  lastPage: string;            // LastPage (URL)
  nextPage?: string | null;    // NextPage (URL)
  previousPage?: string | null;// PreviousPage (URL)
  totalPages: number;          // TotalPages
  totalRecords: number;        // TotalRecords
}
