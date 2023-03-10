using EFCore.Indexes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Indexes.EntityTypeConfigurations;

internal class BlogEntityTypeConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        // primary key
        // by default has b-tree index
        builder.HasKey(blog => blog.Id);


        //------------------------------------------------------------------------ B-tree index ------------------------------------------------------------------------//

        // by default sets non-clustered index
        // to set as clustered use 
        builder
            .HasIndex(blog => blog.CreatedOn)
            .IsClustered(true);



        //--------------------------------------------------------------------- Multi-column index ---------------------------------------------------------------------//

        // multi-column index, also known as a composite index
        builder
            .HasIndex(blog => new { blog.CreatedOn, blog.Name });

        /*
         
         * How a multi-column index works:
         
         * When a multi-column index is created in PostgreSQL, it is stored as a balanced tree data structure that is optimized for efficient searching and retrieval of data.

         * The tree is built based on the values of the indexed columns, and each leaf node of the tree contains a list of tuples (index entries) 
         * that contain the indexed column values and a pointer to the corresponding row in the table.

         * When a query is executed that involves the columns in the multi-column index, 
         * the PostgreSQL query optimizer examines the query and determines whether the multi-column index can be used to speed up the query. 
         * If the optimizer decides to use the index, it traverses the tree to find the leaf nodes that contain the tuples that match the search criteria. 
         * The tuples are then retrieved from the index and the corresponding rows are fetched from the table using the pointers stored in the tuples.
         
         */


        // This can be extended to 'Cover Index'

        /*
         
         * A cover index, also known as a covering index, is a type of index in a database that includes all the columns needed to satisfy a query.
         * When a query is executed, the database management system retrieves data from the index instead of reading from the table.
         * If the index includes all the columns required to satisfy the query, the database does not need to read any data from the table, which can significantly improve the performance of the query.


        * IMPORTANT! Cover indexes can be less efficient for queries that only require a small subset of the columns in the index. Use only for queries that require a large number of columns.
        
        */


        //----------------------------------------------------------------- Generalized Inverted Index -----------------------------------------------------------------//

        builder
            .HasGeneratedTsVectorColumn(
                blog => blog.Search,
                config: "simple",
                blog => new { blog.Name, blog.Description })
        .HasIndex(blog => blog.Search)
        .HasMethod("GIN");

        /*
         
         * The first parameter of the HasGeneratedTsVectorColumn method is a lambda expression that specifies the tsvector column to be generated.
         * In this case, it is blog => blog.Search, which means that the generated tsvector column is named Search and is a property of the Blog entity.
         
         * The second parameter of the HasGeneratedTsVectorColumn method is the configuration for the text search vector. 
         * In this case, it is "simple", which specifies that the text search vector should be created using the simple configuration.
               Types of configurations: 
               * simple: This is the simplest configuration and performs only basic tokenization and stemming.
               * english: This configuration is designed for English-language text and includes more advanced tokenization and stemming rules for English.
               * russian ||-||-||
               * german ||-||-||
               * french ||-||-||
               * spanish ||-||-||
         
         * The third parameter of the HasGeneratedTsVectorColumn method is a lambda expression that specifies the columns whose text search vectors will be used to generate the tsvector column. 
         * In this case, it is blog => new { blog.Name, blog.Description }, which specifies that the Name and Description columns will be used to generate the tsvector column.
         
         * After generating the tsvector column, the HasIndex method is used to create an index on the Search column.
         * The HasMethod method is then used to specify the index method as "GIN", which stands for Generalized Inverted Index.
         

         * Now it can be use over IQueryable with Where clause.
         
                var searchQuery = "some search query";
                var results = dbContext.Blog
                    .Where(e => EF.Functions.Matches(e.Search, searchQuery))
                    .ToList();

        * IMPORTANT! It is used only over IQueryable.

         */
    }
}
